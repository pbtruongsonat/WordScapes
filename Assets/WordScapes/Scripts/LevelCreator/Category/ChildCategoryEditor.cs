using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.IO;
using SFB;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Collections.Generic;
using UnityEngine.WSA;

public class ChildCategoryEditor : MonoBehaviour
{
    public GameObject parentPanel;
    public GameObject childPanel;

    public TextMeshProUGUI title;

    [Header("Create Child Category")]
    public Button createButton;
    public Button editButton;
    [Space]
    public Button saveButton;

    [Header("Delete Option")]
    public GameObject deleteOption;
    public GameObject deletePanel;
    public TextMeshProUGUI notificationDelete;
    public Button deleteButton;
    public Button submitDelete;
    public Button cancelDelete;

    [Header("Component Child Category")]
    public ChildCategory child;
    public TMP_InputField nameCategory;
    [Space]
    public Image bgImage;
    public TextMeshProUGUI fileNameBg;
    public Button chooseBgButton;
    public Sprite newBg;

    private void Start()
    {
        createButton.onClick.AddListener(() => OnCreateCategory());
        editButton.onClick.AddListener(() => OnEditCategory());
        saveButton.onClick.AddListener(() => OnClickSaveCategory());
        
        // Delete Option
        deleteButton.onClick.AddListener(() => OnClickDelete());
        submitDelete.onClick.AddListener(() => DeleteCategory());
        cancelDelete.onClick.AddListener(() => deletePanel.SetActive(false));

        nameCategory.onValueChanged.AddListener((value) => { nameCategory.text = value.ToUpper(); });

        chooseBgButton.onClick.AddListener(() => ChooseBackground());
    }

    private void OnEditCategory()
    {
        title.text = "Edit Child Category";

        child = CategoryManager.Instance.childSelected;
        nameCategory.text = child.name;
        bgImage.sprite = child.backgroundImage;
        fileNameBg.text = child.backgroundImage.name;
        newBg = null;
        deleteOption.SetActive(true);
    }

    private void OnCreateCategory()
    {
        title.text = "Create Child Category";

        child = null;
        newBg = null;
        nameCategory.text = "";
        deleteOption.SetActive(false);
    }

    private void OnClickDelete()
    {
        notificationDelete.text = $"Are you sure you want to delete ChildCategory: \n{child.name} ?";
        deletePanel.SetActive(true);
    }

    private void DeleteCategory()
    {
        CategoryManager.Instance.RemoveChild();
        deletePanel.SetActive(false);
    }

    private void OnClickSaveCategory()
    {
        if (child == null)
        {
            CreateNewCategory();
        }
        else
        {
            SaveChangeCategory();
        }
        this.gameObject.SetActive(false);
    }
    private void CreateNewCategory()
    {
        ChildCategory newChild = ScriptableObject.CreateInstance<ChildCategory>();
        newChild.name = nameCategory.text;
        if(newBg != null)
        {
            newChild.backgroundImage = newBg;
        }
        newChild.listLevelID = new List<int>();
        CategoryManager.Instance.AddNewChild(newChild);
        // Create Asset
        string path = $"Assets/WordScapes/Resources/Data/Category/C_{nameCategory.text}.asset";
        UnityEditor.AssetDatabase.CreateAsset(newChild, path);
        UnityEditor.AssetDatabase.SaveAssets();
    }
    private void SaveChangeCategory()
    {
        if(newBg != null)
        {
            child.backgroundImage = newBg;
        }
        child.name = nameCategory.text;
        CategoryManager.Instance.UpdateOptionCategory();
    }

    // Image 
    private void ChooseBackground()
    {
        var filter = new[]{ new ExtensionFilter("Image File", "png", "img", "jpeg")};
        string []path = StandaloneFileBrowser.OpenFilePanel("Choose Background Image", "Assets/WordScapes/Resources/Background", filter, false);
        if(path.Length > 0 && File.Exists(path[0])){
            fileNameBg.text = Path.GetFileNameWithoutExtension(path[0]);
            newBg = Resources.Load<Sprite>($"Background/{fileNameBg.text}");
            bgImage.sprite = newBg;
            fileNameBg.text = newBg.name;
        }
    }
}
