using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoTextLogDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPrefab;
    [SerializeField]
    private float timeOut;
    [SerializeField]
    private float fadeOutSpeed = 5f;
    [SerializeField]
    private int maxAmountEntries = 8;

    private string pickedUpTextString = "Picked up {0}";
    private string upgradeTextString = "{0}+: {1}";
    private string unlockTextString = "New Weapon: {0}";

    private Player player;

    private Color upgradeTextColor = new Color(12f / 255f, 118f / 255f, 0f, 1f);

    private List<TextLogEntry> textLog = new List<TextLogEntry>();

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        player.OnPickup += AddPickupTextEntry;
        player.OnWeaponUpgrade += AddUpgradeTextEntry;
        player.OnWeaponUnlocked += AddUnlockTextEntry;
    }

    private void OnDisable()
    {
        player.OnPickup -= AddPickupTextEntry;
        player.OnWeaponUpgrade -= AddUpgradeTextEntry;
        player.OnWeaponUnlocked -= AddUnlockTextEntry;
    }

    private void AddUnlockTextEntry(Weapon weapon)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, upgradeTextColor);
        logEntry.SetText(string.Format(unlockTextString, weapon.GetWeaponInfo().DisplayName, weapon.GetWeaponInfo().DisplayName));
        AddAndShowTextLogEntry(logEntry);
    }
    private void AddUpgradeTextEntry(Weapon weapon, UpgradeInfo upgrade)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, upgradeTextColor);
        logEntry.SetText(string.Format(upgradeTextString, weapon.GetWeaponInfo().DisplayName, upgrade.DisplayName));
        AddAndShowTextLogEntry(logEntry);
    }

    private void AddPickupTextEntry(Weapon weapon)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.white);
        logEntry.SetText(string.Format(pickedUpTextString, weapon.GetWeaponInfo().DisplayName));
        AddAndShowTextLogEntry(logEntry);
    }

    private void AddAndShowTextLogEntry(TextLogEntry logEntry) 
    {
        if (textLog.Count == maxAmountEntries)
            FadeOutEntryAfterDelay(0f, fadeOutSpeed, textLog[0]);

        textLog.Add(logEntry);
        logEntry.MoveToIndex(0);

        StartCoroutine(FadeOutEntryAfterDelay(timeOut, 5f, logEntry));
    }

    private IEnumerator FadeOutEntryAfterDelay(float delay, float speed, TextLogEntry entry)
    {
        yield return new WaitForSeconds(delay);
        while (entry.TextMesh.color.a > 0.0f)
        {
            Color modified = entry.TextMesh.color;
            modified.a -= (Time.deltaTime * speed);
            entry.TextMesh.color = modified;
            yield return null;
        }
        entry.Destroy();
        textLog.Remove(entry);
    }
}
