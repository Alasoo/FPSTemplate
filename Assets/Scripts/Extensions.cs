using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;




namespace MyExtensions
{
    public static class Extensions
    {
        public static async Task LerpScale(this Transform transform, Vector3 targetScale, float duration, CancellationToken token)
        {
            var time = 0f;
            var startScale = transform.localScale;
            while (time < duration && !token.IsCancellationRequested)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                transform.localScale = targetScale;
            }
            else
                throw new TaskCanceledException();
        }


        public static async Task LerpPosition(this Transform transform, Vector3 targetPosition, float duration, CancellationToken token)
        {
            var time = 0f;
            var startScale = transform.position;


            while (time < duration && !token.IsCancellationRequested)
            {
                transform.position = Vector3.Lerp(startScale, targetPosition, time / duration);
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                transform.position = targetPosition;
            }
            else
                throw new TaskCanceledException();
        }


        public static async Task LerpAlpha(this CanvasGroup cg, float target, float duration, CancellationToken token)
        {
            var time = 0f;
            var startValue = cg.alpha;
            while (time < duration && !token.IsCancellationRequested)
            {
                cg.alpha = Mathf.Lerp(startValue, target, time / duration);
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                cg.alpha = target;
            }
            else
                throw new TaskCanceledException();
        }


        public static async Task LerpAlpha(this CanvasGroup cg, float target, float delay, float duration, CancellationToken token)
        {
            var time = 0f;


            while (time < delay && !token.IsCancellationRequested)
            {
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            var startValue = cg.alpha;
            time = 0f;
            while (time < duration && !token.IsCancellationRequested)
            {
                cg.alpha = Mathf.Lerp(startValue, target, time / duration);
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                cg.alpha = target;
            }
            else
                throw new TaskCanceledException();
        }


        public static async Task LerpMaster(this AudioMixer audioMixer, float target, float duration, string masterName, CancellationToken token)
        {
            var time = 0f;
            audioMixer.GetFloat(masterName, out float volume);
            var startValue = volume;


            while (time < duration && !token.IsCancellationRequested)
            {
                audioMixer.SetFloat(masterName, Mathf.Lerp(startValue, target, time / duration));
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                audioMixer.SetFloat(masterName, target);
            }
            else
                throw new TaskCanceledException();
        }






        public static async Task LerpImageAlpha(this Image image, float target, float duration, CancellationToken token)
        {
            var time = 0f;
            var startValue = image.color.a;
            while (time < duration && !token.IsCancellationRequested)
            {
                Color color = image.color;
                color.a = Mathf.Lerp(startValue, target, time / duration);
                image.color = color;
                time += Time.deltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                Color color = image.color;
                color.a = target;
                image.color = color;
            }
            else
                throw new TaskCanceledException();
        }


        public static async Task LerpImagesAlpha(this List<Image> images, float target, float duration, CancellationToken token)
        {
            if (images.Count <= 0) return;


            var time = 0f;
            var startValue = images[0].color.a;
            while (time < duration && !token.IsCancellationRequested)
            {
                Color color = images[0].color;
                color.a = Mathf.Lerp(startValue, target, time / duration);
                foreach (Image image in images)
                    image.color = color;


                time += Time.deltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                Color color = images[0].color;
                color.a = target;
                foreach (Image image in images)
                    image.color = color;
            }
            else
                throw new TaskCanceledException();
        }


        public static async Task LerpAlpha(this SpriteRenderer sprite, float target, float duration, CancellationToken token)
        {
            var time = 0f;
            var startValue = sprite.color.a;
            while (time < duration && !token.IsCancellationRequested)
            {
                Color color = sprite.color;
                color.a = Mathf.Lerp(startValue, target, time / duration);
                sprite.color = color;
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                Color color = sprite.color;
                color.a = target;
                sprite.color = color;
            }
            else
                throw new TaskCanceledException();
        }




        public static async Task LerpTimeScale(float target, float duration, CancellationToken token)
        {
            float startValue = Time.timeScale;
            float time = 0f;


            while (time < duration && !token.IsCancellationRequested)
            {
                Time.timeScale = Mathf.Lerp(startValue, target, time / duration);
                time += Time.unscaledDeltaTime; // Usamos unscaledDeltaTime para que no se vea afectado
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                Time.timeScale = target;
            }
            else
                throw new TaskCanceledException();
        }








        public static async Task LerpLight(this Light2D light, float target, float duration, CancellationToken token)
        {
            var time = 0f;
            var startScale = light.intensity;


            while (time < duration && !token.IsCancellationRequested)
            {
                light.intensity = Mathf.Lerp(startScale, target, time / duration);
                time += Time.unscaledDeltaTime;
                await Task.Yield();
            }


            if (!token.IsCancellationRequested)
            {
                light.intensity = target;
            }
        }




    }


}





