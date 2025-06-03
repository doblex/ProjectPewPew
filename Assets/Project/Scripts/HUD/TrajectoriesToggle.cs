using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrajectoriesToggle : MonoBehaviour
{
    [SerializeField] HUD HUD_ref;
    [SerializeField] RectTransform TrajectorySelector_ref;
    [SerializeField] RectTransform TrajectoriesPanel_ref;
    [SerializeField] float TrajectoryClosedSelectorZRotation;
    [SerializeField] float TrajectoryOpenedSelectorZRotation;
    [SerializeField] Vector2 TrajectoriesClosedPanelPosition;
    [SerializeField] Vector2 TrajectoriesOpenedPanelPosition;
    [SerializeField] float OpenPanelTime;
    [SerializeField] int TrajectoriesNumber;
    float timer;
    bool b_SelectorIsRotated;
    bool b_PanelIsOpened;
    public bool b_PanelIsOpenig;
    Quaternion ClosedRotation;
    Quaternion OpenedRotation;

    private void OnEnable()
    {
        if (b_PanelIsOpenig)
        {
            b_PanelIsOpenig = false;
        }
        else
        {
            b_PanelIsOpenig = true;
        }
        OpenedRotation = Quaternion.Euler(0f, 0f, TrajectoryOpenedSelectorZRotation);
        ClosedRotation = Quaternion.Euler(0f, 0f, TrajectoryClosedSelectorZRotation);
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (b_PanelIsOpenig)
        {
            Debug.LogError("attivato");
            if (!b_PanelIsOpened || !b_SelectorIsRotated)
            {
                Debug.LogError("apro");
                if (TrajectorySelector_ref.rotation != OpenedRotation)
                {
                    Debug.LogError("ruoto");
                    TrajectorySelector_ref.rotation = Quaternion.Lerp(ClosedRotation, OpenedRotation, timer / OpenPanelTime);
                }
                else
                {
                    b_SelectorIsRotated = true;
                }

                if (TrajectoriesPanel_ref.anchoredPosition != TrajectoriesOpenedPanelPosition)
                {
                    Debug.LogError("sposto");
                    TrajectoriesPanel_ref.anchoredPosition = Vector2.Lerp(TrajectoriesClosedPanelPosition, TrajectoriesOpenedPanelPosition, timer / OpenPanelTime);
                }
                else
                {
                    Debug.LogError("finito");
                    b_PanelIsOpened = true;
                }
            }
            else
            {
                this.enabled = false;
            }
        }
        else
        {
            if (b_PanelIsOpened || b_SelectorIsRotated)
            {
                Debug.LogError("chiuso");
                if (TrajectorySelector_ref.transform.localRotation != ClosedRotation)
                {
                    TrajectorySelector_ref.transform.localRotation = Quaternion.Lerp(OpenedRotation, ClosedRotation, OpenPanelTime / timer);
                }
                else
                {
                    b_SelectorIsRotated = false;
                }

                if (TrajectoriesPanel_ref.anchoredPosition != TrajectoriesClosedPanelPosition)
                {
                    TrajectoriesPanel_ref.anchoredPosition = Vector2.Lerp(TrajectoriesOpenedPanelPosition, TrajectoriesClosedPanelPosition, OpenPanelTime / timer);
                }
                else
                {
                    b_PanelIsOpened = false;
                }
            }
            else
            {
                HUD_ref.CloseTrajectoriesPanel();
                this.enabled = false;
            }
        }
    }
}
