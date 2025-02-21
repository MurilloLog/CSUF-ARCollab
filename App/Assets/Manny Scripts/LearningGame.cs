using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearningGame : MonoBehaviour
{
    float gameTime;
    public Timer timer;
    public Dictionary<string, string> questions_answers;
    public Button[] answers; //0-3, 0 is top left, 1 top right,
                             //2 bottom left, 3 bottom right
    public ProgressBar progressBar;
    public TMP_Text question;
    [SerializeField] GameObject Tuffy;
    int questionPos;
    int numCorrect;
    string[] answerList = { "Outer Core", "Inner Core", "Mantle", "Crust" };
    

    // Start is called before the first frame update
    void Start()
    {
        numCorrect = 0;
        questions_answers = new Dictionary<string, string>()
        {
            {"What layer is 1.400 miles thick?", "Outer Core" },
            {"What layer is 9,000 F?" , "Outer Core"},
            {"What layer is 9,800 F?" ,"Inner Core"},
            {"Which layer is sandwhiched between the crust and outer core?", "Mantle"},
            {"Which layer is the thickest?","Mantle"},
            {"What layer goes about 19 miles deep on average?","Crust"},
            {"What layer has the consistency of caramel?","Mantle"}
        };

        //Start of the quiz
        //puts first question as question text
        questionPos = 0;
        question.text = questions_answers.ElementAt(questionPos).Key;
        StartQuestion();
      


    }

    // Update is called once per frame
    void Update()
    {
        progressBar.current = questionPos;
    }
    

    //Button functionality

    public void Next()
    {
        questionPos = questionPos + 1;
        if(questionPos >= questions_answers.Count) 
        {
            //checking to see that the question is at max
            //if so then end the game

            Debug.Log("Congrats you have reached the end");
            foreach (Button b in answers)
            {
                b.interactable = false;
            }

            question.text = $"Congrats you got {numCorrect} questions right.";
            //leave this function
            //save the value to maybe be seen in profile
            CreateScoreTextFile();

            return;


        }
        question.text = questions_answers.ElementAt(questionPos).Key;
        timer.timer = timer.MaxTime;
        timer.timerRunning = true;
        foreach(Button button in answers)
        {
            button.interactable = true;
        }
        question.color = new Color(0, 17, 255);
        //clear the text from the buttons
        foreach(Button b in answers)
        {
            b.GetComponentInChildren<TextMeshProUGUI>().text = "Button";
        }
        StartQuestion();
        Tuffy.SetActive(false);
    }

    void StartQuestion()
    {
        List<string> possibleAnswers = GetRandomAnswers();
        List<Button> possibleButtons = new List<Button>(answers);

        //assigns the correct answer to random button
        int correct = Random.Range(0, 4);
        answers[correct].transform.GetComponentInChildren<TextMeshProUGUI>().text =
            questions_answers.ElementAt(questionPos).Value;

        foreach(Button button in answers)
        {
            if(button.transform.GetComponentInChildren<TextMeshProUGUI>().text
                == questions_answers.Values.ElementAt(questionPos))
            {
                possibleButtons.Remove(button);
            }
        }
        //assign the rest of the questions
        //checking to see that possibleAnswers is actually working
        //foreach(var x in possibleAnswers) { Debug.Log(x.ToString()); }

        for(int i = 0; i < possibleAnswers.Count; i++) 
        {
            possibleButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = 
                possibleAnswers[i];
        }
        

    }

    void ShuffleAnswers(List<string> list)
    {
        int n = list.Count;

        while (n > 1) 
        {
            n--;
            int k = Random.Range(0, n+1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private List<string> GetRandomAnswers()
    {
        List<string> possibleAnswers = new List<string>(answerList);
        ShuffleAnswers(possibleAnswers);

        //make sure the correct one is not in List
        string correct = questions_answers.Values.ElementAt(questionPos);
        if (possibleAnswers.Contains(correct))
        {
            possibleAnswers.Remove(correct);
        }

        return possibleAnswers.GetRange(0,3);
    }

    public void CheckAnswer(Button selectedButton)
    {
        string selectedAnswer = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        foreach (Button b in answers)
        {
            ColorBlock colors = b.colors;
            colors.normalColor = Color.white;
            b.colors = colors;
        }

        string correctAnswer = questions_answers.Values.ElementAt(questionPos);
        int correctIndex = System.Array.IndexOf(answers, correctAnswer);
        //if answer is correct
        if (selectedAnswer ==  correctAnswer)
        {
            Debug.Log("Correct!");
            question.color = Color.green;
            numCorrect++;
            //pause the timer
            timer.timerRunning = false;
            //show that it is the correct answer
            //show tuffy saying "bravo"
            Tuffy.SetActive(true);
            Tuffy.GetComponentInChildren<TextMeshProUGUI>().text = "Bravo";
            if (correctIndex >= 0 && correctIndex < answers.Length) 
            {
                ColorBlock colors = answers[correctIndex].colors;
                colors.normalColor = Color.green;
                answers[correctIndex].colors = colors;
            }


        }
        else 
        {
            Debug.Log("Incorrect.");
            question.color= Color.red;
            //still pause the timer
            timer.timerRunning=false;
            //show the correct answer
            //show tuffy correcting.
            Tuffy.SetActive(true);
            Tuffy.GetComponentInChildren<TextMeshProUGUI>().text = $"I am sorry, the correct answer is {correctAnswer}";
            if (correctIndex >= 0 && correctIndex < answers.Length)
            {
                ColorBlock colors = answers[correctIndex].colors;
                colors.normalColor = Color.green;
                answers[correctIndex].colors = colors;
            }
        }

        foreach (Button b in answers)
        {
            b.interactable = false;
        }
    }



    //function for creating text file 
    void CreateScoreTextFile()
    {
        //check to see if file exists
        string path = Application.dataPath + "/UserScore.txt";
        if(!File.Exists(path))
        {
            File.WriteAllText(path, "User Score \n\n");
            Debug.Log("File Created");
        }
        //add text
        string content = $"Last attempt for layer quiz: {numCorrect}" + "\n";
        File.AppendAllText(path, content);
        Debug.Log("File Appended");
    }
}
