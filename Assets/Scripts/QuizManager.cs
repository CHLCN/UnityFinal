using System.Linq;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    private Question[] questions = new Question[] {
        new Question { fact = "0的倒数还是0", isTrue = false },
        new Question { fact = "细菌必须用电子显微镜才能看到", isTrue = false },
        new Question { fact = "古人把山南水北称“阳”，山北水南称“阴”", isTrue = true },
        new Question { fact = "液体深度越深,压强越大", isTrue = true },
        new Question { fact = "连接两点间的线段叫两点间的距离", isTrue = true },
        new Question { fact = "在化学变化中,原子不能再分", isTrue = true },
        new Question { fact = "南宋商品经济发达,出现了世界上最早的纸币“交子”。", isTrue = false },
    };
    private static List<Question> unansweredQuestions;
    public GameObject quizPanel;
    public GameObject gameOverPanel;

    private Question currentQuestion;

    [SerializeField]
    private Text factText = null;

    [SerializeField]
    private float timeAfterQuestion = 3f;

    void Start()
    {
        //使问题尽量不重复
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        SetCurrentQuestion();
        Debug.Log(questions);
    }

    //随机选择问题
    public void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        factText.text = currentQuestion.fact;

    }

    IEnumerator waiting()
    {

        yield return new WaitForSeconds(100 * Time.deltaTime); // 一定要加上Time.deltatime 未解决
        //quizPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    IEnumerator gameOver()
    {
        yield return new WaitForSeconds(timeAfterQuestion * Time.deltaTime);
        //quizPanel.SetActive(false);
        Application.Quit();

    }

    public void UserSelectTrue()
    {
        if (currentQuestion.isTrue)
        {
            Debug.Log("CORRECT");
            StartCoroutine(waiting());
        }
        else
        {
            Debug.Log("WRONG");
            StartCoroutine(gameOver());

        }


    }

    public void UserSelectFalse()
    {
        if (!currentQuestion.isTrue)
        {
            Debug.Log("CORRECT");
            StartCoroutine(waiting());

        }
        else
        {
            Debug.Log("WRONG");
            StartCoroutine(gameOver());


        }

    }
}
