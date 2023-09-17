using System.Collections;
using System.Linq;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public string NameAndType;
    [SerializeField] private WhackAMole mainGame;
    [SerializeField][Range(0f, 1f)] private float lerpTime;
    [SerializeField] private Color textColor;
    public Transform spawnLoc;
    public GameObject correctEffect, wrongEffect;

    // Start is called before the first frame update
    private void Start()
    {
        mainGame = FindObjectOfType<WhackAMole>();
        StartCoroutine(GoAway());
    }

    // Update is called once per frame
    private void Update()
    {
        this.transform.position = spawnLoc.position;
        this.transform.rotation = spawnLoc.rotation;
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked ON");
        if (NameAndType == mainGame.questions_answers.ElementAt(mainGame.question_index).Value)
        {
            int index = Random.Range(0, mainGame.questions_answers.Count);
            //change value in dictionary to true to mark that it has been used before
            //check to see if it is the same
            if (mainGame.questions_answers.ElementAt(index).Key != mainGame.question.text)
            {
                //since it is not the same, question will change
                mainGame.question.text = mainGame.questions_answers.ElementAt(index).Key;
                mainGame.scoreNum += 10;
                Instantiate(correctEffect, spawnLoc.transform.position, Quaternion.identity);

            }
            else
            {
                do
                {
                    index = Random.Range(0, mainGame.questions_answers.Count);
                } while (mainGame.questions_answers.ElementAt(index).Key == mainGame.question.text);

            }
            //add to score
            Destroy(transform.parent.gameObject);
            Debug.Log("CORRECT");
        }
        else
        {
            //remove from score
            if(mainGame.scoreNum > 0)
            {
                mainGame.scoreNum -= 10;
            }
            //change text color to be red then back normal
            //mainGame.StartCoroutine(SmoothColorChange(mainGame.question, Color.black, Color.red, 1f, 1f));
            Instantiate(wrongEffect, spawnLoc.transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
            Debug.Log("WRONG!");
        }
    }

    //make the clickables last for a few seconds
    private IEnumerator GoAway()
    {
        yield return new WaitForSeconds(3);
        Destroy(transform.parent.gameObject);
    }
}