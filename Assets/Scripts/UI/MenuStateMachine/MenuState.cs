
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyExtensions;
using UnityEngine;


namespace MenuStateMachineSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuState : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup panel;

        protected CancellationTokenSource ctx1;

        void OnDestroy()
        {
            ctx1?.Cancel();
        }


#if UNITY_EDITOR
        void OnValidate()
        {
            if (!panel)
                panel = GetComponent<CanvasGroup>();
        }
#endif


        public virtual void Init()
        {
            gameObject.SetActive(false);
        }

        public virtual void Enter()
        {
            FadeIn();
        }

        public virtual void Exit()
        {
            FadeOut();
        }



        protected void FadeIn(CanvasGroup cg = null)
        {
            if (cg == null)
                cg = panel;
            if (cg == null) return;

            cg.interactable = true;

            ctx1?.Cancel();
            ctx1 = new CancellationTokenSource();
            cg.alpha = 0;
            cg.gameObject.SetActive(true);
            _ = cg.LerpAlpha(1, .2f, ctx1.Token);
        }

        protected async void FadeOut(CanvasGroup cg = null)
        {
            if (cg == null)
                cg = panel;
            if (cg == null) return;

            cg.interactable = false;

            ctx1?.Cancel();
            ctx1 = new CancellationTokenSource();
            var currentToken = ctx1.Token;

            try
            {
                await cg.LerpAlpha(0, .2f, currentToken);
                if (!currentToken.IsCancellationRequested)
                {
                    cg.gameObject.SetActive(false);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Animación cancelada por TaskCanceledException");
            }
        }

        public virtual void ForceClose()
        {
            panel.gameObject.SetActive(false);
            panel.interactable = false;
            panel.alpha = 0;
        }
        public virtual void ForceOpen()
        {
            panel.gameObject.SetActive(true);
            panel.interactable = true;
            panel.alpha = 1;
        }
    }

}
