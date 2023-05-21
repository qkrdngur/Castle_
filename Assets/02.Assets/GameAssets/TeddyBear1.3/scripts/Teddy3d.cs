using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teddy3d : MonoBehaviour
{
    //GameObject Teddy;
    Animator anim;
    Rigidbody rigid;
    Rigidbody rig;
    Transform trans;
    float jumpforce;
    bool gr;
    float speed;
    Vector3 v3Velocity;
    Vector3 v3VelocityAUX;
    Vector3 forcedir;
    RaycastHit hit;
    RaycastHit hit2;
    bool active;
    bool dead;
    bool walk;
    float randomTime;
    float timeCounter;
    float fallCounter;
    Transform Pelvis;


    void Start()
    {
        Pelvis = transform.Find("ROOT/Teddy 1/Teddy Pelvis");
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        jumpforce = 100f;
        forcedir = new Vector3(1f, 0f, 0f);
        active = true;
        dead = false;
        randomTime = 12f;
        timeCounter = 0f;
        fallCounter = 0f;
        anim.SetInteger("idle", 300);

        Transform[] allChildren = transform.GetChild(0).transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<Rigidbody>() != null)
            {
                child.GetComponent<Rigidbody>().useGravity = false;
                child.GetComponent<Rigidbody>().isKinematic = true;
            }
            if (child.GetComponent<BoxCollider>() != null) child.GetComponent<BoxCollider>().enabled = false;
            if (child.GetComponent<CapsuleCollider>() != null) child.GetComponent<CapsuleCollider>().enabled = false;
            if (child.GetComponent<SphereCollider>() != null) child.GetComponent<SphereCollider>().enabled = false;
        }        
    }


    private void Update()
    {
        //STAND UP
        if (active && dead && Input.anyKey)
        {
            StartCoroutine("StandUp");
            active = false;            
        }
        //if (dead && Pelvis.GetComponent<Rigidbody>().velocity.magnitude < 0.1f) active = true;

        if (!dead)
        {
            //IDLES
            if (timeCounter > randomTime)
            {
                anim.SetInteger("idle", Random.Range(0, 1200));
                randomTime = Random.Range(1, 12);
                timeCounter = 0f;
            }
            timeCounter += Time.deltaTime;

            //WALK
            if (Input.GetKey(KeyCode.LeftShift))
                walk = true;
            else
                walk = false;
            anim.SetBool("walk", walk);

            //TRANSLATE
            v3Velocity = rigid.velocity;
            speed = v3Velocity.magnitude;
            anim.SetFloat("run", Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(KeyCode.W) && gr == true && speed < 5f && active == true)
            {
                if (walk == true) rigid.velocity = forcedir * -1f;
                if (walk == false) rigid.velocity = forcedir * -4f;
            }
            if (Input.GetKey(KeyCode.S) && gr == true && speed <= 1f && speed > -1.1f && active == true)
            {
                if (walk == true) rigid.velocity = forcedir * 1f;
                if (walk == false) rigid.velocity = forcedir * 3f;
            }
            anim.SetFloat("speed", speed);

            //TURN       
            if (Input.GetKey(KeyCode.D) && gr == true && active == true)
            {
                trans.Rotate(new Vector3(0f, 3f, 0f));
                if (anim.GetFloat("run") == 0f)
                    anim.SetInteger("turn", 1);
            }
            else
            {
                if (Input.GetKey(KeyCode.A) && gr == true && active == true)
                {
                    trans.Rotate(new Vector3(0f, -3f, 0f));
                    if (anim.GetFloat("run") == 0f)
                        anim.SetInteger("turn", -1);
                }
                else anim.SetInteger("turn", 0);
            }

            //JUMP
            if (Input.GetKey(KeyCode.Space) && gr == true && active == true)
            {
                active = false;
                //static
                if (anim.GetFloat("run") <= 0f)
                {
                    StartCoroutine("Activate", 0.75f);
                    anim.Play("jumpin");
                    rigid.AddForce(new Vector3(0f, 1f, 0f) * jumpforce * 1.5f, ForceMode.Acceleration);
                }
                //run
                if (anim.GetFloat("run") > 0f)
                {
                    StartCoroutine("Activate", 1f);
                    anim.Play("jumprunin");
                    rigid.AddForce((trans.forward * 0.07f + new Vector3(0f, 2.5f, 0f)) * jumpforce, ForceMode.Acceleration);
                }
            }

            //RAGDOLL       
            if (speed > 11)
            {
                v3VelocityAUX = v3Velocity;
                active = false;
                GetComponent<Collider>().enabled = false;
                anim.enabled = false;
                //rigid.isKinematic = true;
                fallCounter = 0f;
                CameraTeddy3d.dead = true;

                Transform[] allChildren = transform.GetChild(0).transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.GetComponent<Rigidbody>() != null)
                    {
                        child.GetComponent<Rigidbody>().useGravity = true;
                        child.GetComponent<Rigidbody>().isKinematic = false;
                        child.GetComponent<Rigidbody>().velocity = v3VelocityAUX;
                    }
                    if (child.GetComponent<BoxCollider>() != null) child.GetComponent<BoxCollider>().enabled = true;
                    if (child.GetComponent<CapsuleCollider>() != null) child.GetComponent<CapsuleCollider>().enabled = true;
                    if (child.GetComponent<SphereCollider>() != null) child.GetComponent<SphereCollider>().enabled = true;
                }
                StartCoroutine("Activate", 3f);

                dead = true;
            }
        }

        //RESTART
        if (Input.GetKeyDown(KeyCode.P))
        {
            StopAllCoroutines();
            Transform[] allChildren = transform.GetChild(0).transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<Rigidbody>() != null)
                {
                    child.GetComponent<Rigidbody>().useGravity = false;
                    child.GetComponent<Rigidbody>().isKinematic = true;
                    child.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                if (child.GetComponent<BoxCollider>() != null) child.GetComponent<BoxCollider>().enabled = false;
                if (child.GetComponent<CapsuleCollider>() != null) child.GetComponent<CapsuleCollider>().enabled = false;
                if (child.GetComponent<SphereCollider>() != null) child.GetComponent<SphereCollider>().enabled = false;
            }
            CameraTeddy3d.dead = false;
            trans.position = (new Vector3(0f, 0.1f, 0f));
            trans.localRotation = Quaternion.Euler(0, 0, 0);
            rigid.velocity = (new Vector3(0f, 0f, 0f));
            anim.Play("static");
            anim.SetFloat("run", 0f);
            anim.SetBool("walk", false);
            active = true;
            dead = false;
            GetComponent<Collider>().enabled = true;
            anim.enabled = true;
            //rigid.isKinematic = false;
            fallCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        //CHECK GROUNDED       
        if (Physics.Raycast(trans.position + new Vector3(0.18f, 0.05f, 0.15f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(0.18f, 0.05f, -0.15f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(-0.18f, 0.05f, 0.15f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(-0.18f, 0.05f, -0.15f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(-0.087f, 0.075f, 0f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(0.087f, 0.075f, 0f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(0f, 0.075f, -0.08f), Vector3.down, 0.085f)
                || Physics.Raycast(trans.position + new Vector3(0f, 0.075f, 0.08f), Vector3.down, 0.085f))
        {
            anim.SetBool("grounded", true);
            gr = true;
            fallCounter = 0f;           
        }
        else
        {
            anim.SetBool("grounded", false);
            gr = false;
            fallCounter += Time.deltaTime;
        }

        // SET THE FORCE DIRECTION       
        if (Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f), Vector3.down, out hit))
        {
            forcedir = -Vector3.Cross(Vector3.Cross(hit.normal, trans.forward), hit.normal);
            forcedir.Normalize();
        }

        //HITS       
        if (Physics.Raycast(trans.position + new Vector3(-0.2f, 0.35f, 0f), trans.forward, out hit2, 0.33f)
             || Physics.Raycast(trans.position + new Vector3(-0.2f, 1f, 0f), trans.forward, out hit2, 0.33f)
             || Physics.Raycast(trans.position + new Vector3(0.2f, 0.35f, 0f), trans.forward, out hit2, 0.33f)
             || Physics.Raycast(trans.position + new Vector3(0.2f, 1f, 0f), trans.forward, out hit2, 0.33f))
        {
            if (hit2.transform.tag == "enemy")
            {
                rigid.AddForce(trans.forward * -5f, ForceMode.Impulse);
                active = false;
                anim.Play("fall2");
                StartCoroutine("Activate", 3f);
            }
        }
        if (Physics.Raycast(trans.position + new Vector3(-0.2f, 0.35f, 0f), -trans.forward, out hit2, 0.33f)
             || Physics.Raycast(trans.position + new Vector3(-0.2f, 1f, 0f), -trans.forward, out hit2, 0.33f)
             || Physics.Raycast(trans.position + new Vector3(0.2f, 0.35f, 0f), -trans.forward, out hit2, 0.33f)
             || Physics.Raycast(trans.position + new Vector3(0.2f, 1f, 0f), -trans.forward, out hit2, 0.33f))
        {
            if (hit2.transform.tag == "enemy")
            {
                rigid.AddForce(trans.forward * 5f, ForceMode.Impulse);
                active = false;
                anim.Play("fall1");
                StartCoroutine("Activate", 3f);
            }
        }        
    }

    IEnumerator Activate(float wait)
    {
        yield return new WaitForSeconds(wait);
        active = true;
    }
    



    IEnumerator StandUp()
    {
        //yield return new WaitForSeconds(4f);


        Transform[] allChildren = transform.GetChild(0).transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<Rigidbody>() != null)
            {
                child.GetComponent<Rigidbody>().useGravity = false;
                child.GetComponent<Rigidbody>().isKinematic = true;
                child.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (child.GetComponent<BoxCollider>() != null)     child.GetComponent<BoxCollider>().enabled = false;
            if (child.GetComponent<CapsuleCollider>() != null) child.GetComponent<CapsuleCollider>().enabled = false;
            if (child.GetComponent<SphereCollider>() != null)  child.GetComponent<SphereCollider>().enabled = false;
        }

        transform.position = Pelvis.transform.position + Vector3.up * -0.25f;
        GetComponent<Collider>().enabled = true;
        GetComponent<Animator>().enabled = true;
        CameraTeddy3d.dead = false;
        rigid.velocity = Vector3.zero;
        Pelvis.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        anim.SetBool("walk",false);
        anim.SetFloat("run", 0f);
        speed = 0f;

        if (Vector3.Angle(Pelvis.up, Vector3.up) > 90)  //face down
        {
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-Pelvis.transform.right, Vector3.up));
            transform.position -= transform.up * 0.25f;
            anim.Play("Forwardstandup");
        }
        else    //face up
        {
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Pelvis.transform.right, Vector3.up));
            transform.position += transform.up * 0.25f;
            anim.Play("Backwardsstandup");
        }
        yield return new WaitForSeconds(2f);

        active = true;
        dead = false;
    }

}