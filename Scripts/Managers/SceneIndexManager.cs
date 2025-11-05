using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIndexManager : MonoBehaviour
{
    public static SceneIndexManager Instance;
    public int sceneIndex;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        SetSceneIndexBySceneName(SceneManager.GetActiveScene().name);
    }
    private void SetSceneIndexBySceneName(string input)
    {
        // Tìm vị trí của ký tự '_' cuối cùng
        int lastUnderscoreIndex = input.LastIndexOf('_');

        // Kiểm tra nếu có ký tự '_' trong chuỗi
        if (lastUnderscoreIndex == -1)
        {
            Debug.LogError("Ten Scene khong co '_'");
        }

        // Lấy phần chuỗi sau ký tự '_' cuối cùng
        string numberPart = input.Substring(lastUnderscoreIndex + 1);

        // Chuyển đổi phần chuỗi thành số nguyên
        if (int.TryParse(numberPart, out int result))
        {
            sceneIndex = result;
        }
        else
        {
            Debug.LogError("Phần chuỗi sau ký tự '_' không phải là một số hợp lệ.");
        }
    }
}
