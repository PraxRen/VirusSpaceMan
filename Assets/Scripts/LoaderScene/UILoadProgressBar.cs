using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UILoadProgressBar : MonoBehaviour, ISceneLoaderHandler
{
    [SerializeField] private float _speed;

    private Image _image;

    public event Action<ISceneLoaderHandler> Loaded;

    private void Awake() 
    {
        _image = GetComponent<Image>();
    }

    public void HandleLoadScene() => StartCoroutine(UpdateLoadBar());

    private IEnumerator UpdateLoadBar()
    {
        while (Mathf.Approximately(_image.fillAmount, 1f) == false)
        {
            _image.fillAmount = Mathf.MoveTowards(_image.fillAmount, SceneLoader.GetLoadProgress(), _speed * Time.deltaTime);
            yield return null;
        }

        Loaded?.Invoke(this);
    }
}