using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panelContainer;
    [SerializeField] private List<CharacterSO> availableCharacters;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Button characterSelectButton;

    private void Awake() {
        panelContainer.SetActive(false);

        foreach (var character in availableCharacters) {
            Button btn = Instantiate(characterSelectButton, buttonContainer);
            btn.gameObject.SetActive(true);

            Transform iconTrans = btn.transform.Find("Icon");
            if (iconTrans != null && iconTrans.TryGetComponent<Image>(out var img)) {
                img.sprite = character.Icon;
            }

            btn.onClick.AddListener(() => {
                CharacterManager.SelectedCharacter = character;

                Loader.Load(Loader.Scene.GameScene);
            });
        }
    }

    public void OpenPanel() {
        panelContainer.SetActive(true);
    }

    public void ClosePanel() {
        panelContainer.SetActive(false);
    }
}
