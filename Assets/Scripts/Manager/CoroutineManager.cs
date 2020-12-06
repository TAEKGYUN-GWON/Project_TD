using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverStory
{
    public class MicroCoroutine
    {
        List<IEnumerator> _coroutines = new List<IEnumerator>();

        public void AddCoroutine(IEnumerator enumerator)
        {
            _coroutines.Add(enumerator);
        }

        public void Run()
        {
            int i = 0;
            while (i < _coroutines.Count)
            {
                if (!_coroutines[i].MoveNext())
                {
                    _coroutines.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
    }

    public class CoroutineManager : Singleton<CoroutineManager>
    {

        MicroCoroutine _updateMicroCoroutine = new MicroCoroutine();
        MicroCoroutine _fixedUpdateMicroCoroutine = new MicroCoroutine();
        MicroCoroutine _endOfFrameMicroCoroutine = new MicroCoroutine();

        public static void StartUpdateCoroutine(IEnumerator routine)
        {
            if (Instance == null)
                return;
            Instance._updateMicroCoroutine.AddCoroutine(routine);
        }

        public static void StartFixedUpdateCoroutine(IEnumerator routine)
        {
            if (Instance == null)
                return;
            Instance._fixedUpdateMicroCoroutine.AddCoroutine(routine);
        }

        public static void StartEndOfFrameCoroutine(IEnumerator routine)
        {
            if (Instance == null)
                return;
            Instance._endOfFrameMicroCoroutine.AddCoroutine(routine);
        }

        void Awake()
        {
            StartCoroutine(RunUpdateMicroCoroutine());
            StartCoroutine(RunFixedUpdateMicroCoroutine());
            StartCoroutine(RunEndOfFrameMicroCoroutine());
        }

        IEnumerator RunUpdateMicroCoroutine()
        {
            while (true)
            {
                yield return null;
                _updateMicroCoroutine.Run();
            }
        }

        IEnumerator RunFixedUpdateMicroCoroutine()
        {
            var fu = YieldInstructionCache.WaitForFixedUpdate;
            while (true)
            {
                yield return fu;
                _fixedUpdateMicroCoroutine.Run();
            }
        }

        IEnumerator RunEndOfFrameMicroCoroutine()
        {
            var eof = YieldInstructionCache.WaitForEndOfFrame;
            while (true)
            {
                yield return eof;
                _endOfFrameMicroCoroutine.Run();
            }
        }
    }
}
