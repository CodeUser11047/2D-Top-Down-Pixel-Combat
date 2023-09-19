using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : Singleton<DialogManager>
{
    [SerializeField] private Image TalkContanier;
    public GameObject Tagger;
    public bool istalking = false;
    [SerializeField] private TextAsset dialogDateFile;
    [SerializeField] private Image leftSprite;
    [SerializeField] private Image rightSprite;
    [SerializeField] private Image CursorSprite;
    [SerializeField] private TMP_Text talkerNameText;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text Select1Text;
    [SerializeField] private TMP_Text Select2Text;
    [SerializeField] private Button Select1Button;
    [SerializeField] private Button Select2Button;
    //角色图片列表
    [SerializeField] private List<Sprite> sprites = new();
    [SerializeField] private Button nextButton;
    [SerializeField] private float fadeSpeed = 1f;
    private IEnumerator fadeBlack;
    private IEnumerator fadeClear;

    private bool canSkip = false;
    private bool willEnd = false;
    private int dialogIndex = 0;
    /// <summary>
    /// 对话文本，按行分割；
    /// </summary>
    private string[] dialogRows;
    //根据角色名称对应图片字典
    Dictionary<string, Sprite> imageDic = new();

    protected override void Awake()
    {
        base.Awake();

        imageDic["村长"] = sprites[0];
        imageDic["女神"] = sprites[1];
        imageDic["玩家"] = sprites[2];
        imageDic["对话板"] = sprites[3];

        canSkip = false;
        istalking = false;

    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSkip)
        {
            ShowDialogRow();
        }
        if (Input.GetKeyDown(KeyCode.E) && willEnd && canSkip)
        {
            GetEndOption();
        }
    }
    /// <summary>
    /// 外部调用来更新对话文本；
    /// </summary>
    /// <param name="text">csv对话文件</param>
    public void StartTalk(TextAsset text)
    {
        Tagger.SetActive(false);
        istalking = true;

        TalkContanier.gameObject.SetActive(true);
        dialogDateFile = text;

        CursorSprite.gameObject.SetActive(false);
        Cursor.visible = true;


        ReadText(dialogDateFile);

        //渐渐加大
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0.8f, false));


        willEnd = false;
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(delegate { OnNextButtonEvent(); });
    }

    ///<summary>
    ///更新角色姓名以及对话内容
    ///</summary>
    ///<param name="_name">角色名字</param>
    ///<param name="_atLeft">对话内容</param>
    private void UpdateText(string _name, string _text)
    {
        talkerNameText.text = _name;
        dialogText.text = _text;
    }
    ///<summary>
    ///更新图片信息
    ///</summary>
    ///<param name="_name">角色名字</param>
    ///<param name="_atLeft">是否出现在左侧</param>

    private void UpdateImage(string _name, string _position)
    {
        if (_position == "左")
        {
            leftSprite.sprite = imageDic[_name];
        }
        else if (_position == "右")
        {
            rightSprite.sprite = imageDic[_name];
        }
    }


    private void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
    }
    /// <summary>
    /// 
    /// </summary>
    private void ShowDialogRow()
    {
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');

            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                nextButton.gameObject.SetActive(true);
                dialogText.gameObject.SetActive(true);
                Select1Text.gameObject.SetActive(false);
                Select2Text.gameObject.SetActive(false);

                UpdateText(cells[2], cells[4]);
                UpdateImage(cells[2], cells[3]);

                dialogIndex = int.Parse(cells[5]);
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                string[] cells2 = dialogRows[i + 1].Split(',');

                canSkip = false;
                Select1Text.gameObject.SetActive(true);
                Select2Text.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                dialogText.gameObject.SetActive(false);


                Select1Text.text = cells[4];
                Select2Text.text = cells2[4];
                Select1Button.onClick.AddListener(delegate { GetOption(int.Parse(cells[5])); });
                Select2Button.onClick.AddListener(delegate { GetOption(int.Parse(cells2[5])); });
                break;
            }
            if (cells[0] == "End" && int.Parse(cells[1]) == dialogIndex)
            {
                nextButton.gameObject.SetActive(true);
                dialogText.gameObject.SetActive(true);
                Select1Text.gameObject.SetActive(false);
                Select2Text.gameObject.SetActive(false);

                UpdateText(cells[2], cells[4]);
                UpdateImage(cells[2], cells[3]);

                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(delegate { GetEndOption(); });
                willEnd = true;
                istalking = false;
                break;
            }
        }
    }

    //以下是一些委托事件；
    public void GetOption(int id)
    {
        dialogIndex = id;
        ShowDialogRow();
        canSkip = true;
    }

    public void GetEndOption()
    {
        istalking = false;

        CursorSprite.gameObject.SetActive(true);
        Cursor.visible = false;

        for (int i = 0; i < TalkContanier.transform.childCount; i++)
        {
            TalkContanier.transform.GetChild(i).gameObject.SetActive(false);
        }
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, true));

        dialogIndex = 0;
    }

    public void OnNextButtonEvent()
    {
        ShowDialogRow();
    }

    private IEnumerator FadeRoutine(float targetAlpha, bool isOut)
    {
        while (!Mathf.Approximately(TalkContanier.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(TalkContanier.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            TalkContanier.color = new Color(TalkContanier.color.r, TalkContanier.color.g, TalkContanier.color.b, alpha);
            yield return null;
        }

        if (isOut)
        {
            TalkContanier.transform.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < TalkContanier.transform.childCount; i++)
            {
                TalkContanier.transform.GetChild(i).gameObject.SetActive(true);
            }

            ShowDialogRow();

            canSkip = true;
        }
    }
}
