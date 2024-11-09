using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wait : MonoBehaviour
{

    public GameObject Mission1StartPage;


    // Start is called before the first frame update
    void Start()
    {
        if (Mission1StartPage != null) {

            Mission1StartPage.SetActive(false);
        }
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart() {

        yield return new WaitForSeconds(3);

        if (Mission1StartPage != null) {
            Mission1StartPage.SetActive(true);
        }
    }

}
