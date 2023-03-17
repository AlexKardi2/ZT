using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movie : MonoBehaviour
{
    //Loading prepetaions
    List<CombatAction> combatLog = Status.combatLog;
    public GameObject bulletPrefab;
    private GameObject bullet;

    //Settings for the movie
    int divider = 100;
    int numenator = 0;
    Vector3 summator;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
        if (Status.Current != "movie") return;
        CombatAction thisAction = combatLog[Status.MovieAct];

        if (thisAction.action == "move")
        {

            if (summator == Vector3.zero)
            {
                //print("Action"+Status.playAct+" moving to "+thisAction.place[0]+" "+ thisAction.place[1]);
                
                summator.x = (CoordArray.cArray[(combatLog[Status.MovieAct].place[0]), (combatLog[Status.MovieAct].place[1]), 0] - combatLog[Status.MovieAct].subject.transform.position.x) / divider;
                summator.y = (CoordArray.cArray[(combatLog[Status.MovieAct].place[0]), (combatLog[Status.MovieAct].place[1]), 1] - combatLog[Status.MovieAct].subject.transform.position.y) / divider;
            }

            combatLog[Status.MovieAct].subject.transform.position += summator;

            if (++numenator==divider)
            {
                numenator = 0;
                summator = Vector3.zero;
                Status.NextMovieAct();
            }
            
        } 
        else if (thisAction.action == "attack")
        {
            if (summator == Vector3.zero)
            {
                Vector3 target;

                if (thisAction.target == null)
                {
                    target = new Vector3(CoordArray.cArray[thisAction.place[0], thisAction.place[1], 0], CoordArray.cArray[thisAction.place[0], thisAction.place[1], 1],0);
                    summator.x = (CoordArray.cArray[thisAction.place[0], thisAction.place[1], 0] - thisAction.subject.transform.position.x) / divider;
                    summator.y = (CoordArray.cArray[thisAction.place[0], thisAction.place[1], 1] - thisAction.subject.transform.position.y) / divider;
                }
                else
                {
                    target = thisAction.target.transform.position;
                    summator.x = (thisAction.target.transform.position.x - thisAction.subject.transform.position.x) / divider;
                    summator.y = (thisAction.target.transform.position.y - thisAction.subject.transform.position.y) / divider;
                }

                Vector3 bulletPosition = thisAction.subject.transform.position;
                bulletPosition.y += 0.1f;
                bullet = Instantiate(bulletPrefab, bulletPosition, bulletPrefab.transform.rotation);
                bullet.transform.LookAt(target);

                int minus=1;
                if (bullet.transform.rotation.eulerAngles.y < 180)
                    minus = -1;
                Quaternion correctOrientation = Quaternion.Euler(0,0,(minus*bullet.transform.rotation.eulerAngles.x));
                bullet.transform.rotation = correctOrientation;

            }

            bullet.transform.position += summator;

            if (++numenator == divider)
            {
                Destroy(bullet);
                if (thisAction.DamageDone > 0)
                {
                    thisAction.target.OverheadText.ShowRed("-" + thisAction.DamageDone);
                    thisAction.target.OverheadText.ShowHP(thisAction.HPAfter);
                }
                numenator = 0;
                summator = Vector3.zero;

                Status.NextMovieAct();
            }
        }
        else if (thisAction.action == "wait")
        {
            thisAction.subject.OverheadText.ShowGreen("+" + thisAction.apCost + " temporal AC");
            Status.NextMovieAct();
        }
        
    }
}
