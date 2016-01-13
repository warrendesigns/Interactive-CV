using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //objects/classes
    private new Camera camera;
    private GameObject release;
    private Animator animator;
    private UIController uiController;
    private SpawnController spawnController;
    private SpriteRenderer applicationRenderer;
    private Color developColor = new Color(50.0f / 255.0f, 73.0f / 255.0f, 92.0f / 255.0f);
    private Color testColor = new Color(116.0f / 255.0f, 51.0f / 255.0f, 51.0f / 255.0f);

    //private members/variables.
    private int _days;
    private int _money;
    private int _bugs;
    private int _totalBugs;
    private int _currentSkillLevel;
    private bool _testing;
    private float _experience;
    private float _experienceToLevel;
    private float _timerDays;
    private float _timerDevelop;
    private float _timerRelease;
    private float _timerAnimation;
    private string[] _skillLevel = new string[] { "Beginner", "Familiar", "Proficient", "Expert", "Master" };

    //properties/accessors
    //these hold additional logic which updates UI elements/other members, where appropriate.
    public SpriteRenderer ApplicationRenderer
    {
        get
        {
            return applicationRenderer;
        }
    } //read-only
    public int Days
    {
        get
        {
            return _days;
        }
        set
        {
            _days = value;
            uiController.UpdateDaysUI(_days);
        }
    }
    public int Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            uiController.UpdateMoneyUI(_money);
        }
    }
    public int Bugs
    {
        get
        {
            return _bugs;
        }
        set
        {
            _bugs = value;

            if (Testing)
            {
                uiController.UpdateBugUI(_bugs, TotalBugs, true);
            }
        }
    }
    public int TotalBugs
    {
        get
        {
            return _totalBugs;
        }
        set
        {
            _totalBugs = value;

            if(!Testing)
            {
                Bugs = value;
            }

            uiController.UpdateBugUI(Bugs, _totalBugs, false);
        }
    }
    public int CurrentSkillLevel
    {
        get
        {
            return _currentSkillLevel;
        }
        set
        {
            _currentSkillLevel = value;
            uiController.UpdateSkillUI(this[_currentSkillLevel]);
        }
    }
    public bool Testing
    {
        get
        {
            return _testing;
        }
        set
        {
            _testing = value;
        }
    }
    public float Experience
    {
        get
        {
            return _experience;
        }
        set
        {
            if (value > ExperienceToLevel)
            {
                _experience = ExperienceToLevel;
            }
            else
            {
                _experience = value;
            }
            uiController.UpdateExperienceUI(_experience, ExperienceToLevel);
        }
    }
    public float ExperienceToLevel
    {
        get
        {
            return _experienceToLevel;
        }
        set
        {
            _experienceToLevel = value;
        }
    }
    public float TimerDays
    {
        get
        {
            return _timerDays;
        }
        set
        {
            _timerDays = value;
        }
    }
    public float TimerDevelop
    {
        get
        {
            return _timerDevelop;
        }
        set
        {
            _timerDevelop = value;
        }
    }
    public float TimerRelease
    {
        get
        {
            return _timerRelease;
        }
        set
        {
            _timerRelease = value;
        }
    }
    public float TimerAnimation
    {
        get
        {
            return _timerAnimation;
        }
        set
        {
            _timerAnimation = value;
        }
    }
    public string this[int i]
    {
        get
        {
            return _skillLevel[i];
        }
    } //read-only

    //called before start: get references
    private void Awake()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        release = (GameObject)Resources.Load("Release");
        animator = GetComponent<Animator>();
        uiController = GetComponent<UIController>();
        spawnController = GameObject.Find("Spawner").GetComponent<SpawnController>();
        applicationRenderer = GameObject.Find("Application").GetComponent<SpriteRenderer>();
    }

    //initalise members
    private void Start()
    {
        Days = 60;
        Money = 200;
        Bugs = 0;
        TotalBugs = 0;
        CurrentSkillLevel = 0;
        ExperienceToLevel = 100; //initialise ExperienceToLevel before Experience so that ExperienceBar displays correctly. 
        Experience = 0;
        Testing = false;
        TimerDays = 3.0f;
        TimerDevelop = 0.0f;
        TimerAnimation = 0.15f;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        if (TimerDays > 0)
        {
            TimerDays -= Time.deltaTime;
            if (TimerDays <= 0)
            {
                Days--;
                Money -= Random.Range(15, 25);
                TimerDays = 3.0f;
            }
        }

        //Input.GetButton:      returns true EVERY frame the button is held down
        //Input.GetButtonUp:    returns true the FIRST frame the button is released
        //Input.GetButtonDown:  returns true the FIRST frame the button is held down

        if(!Testing)
        {
            if (Input.GetButtonUp("MouseLeft"))
            {
                applicationRenderer.color = Color.white;

                CalculateBugs();
            }
            if (Input.GetButtonDown("MouseRight"))
            {
                Test();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("MouseLeft"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);

            if(hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Application")
                {
                    Release();
                }
                else
                {
                    Develop();
                }
            }
            else
            {
                Develop();
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------
    private void Develop()
    {
        applicationRenderer.color = developColor;

        TimerDevelop += Time.deltaTime;

        TimerAnimation -= Time.deltaTime;
        if (TimerAnimation <= 0.0f)
        {
            UpdateAnimation();
        }

        Experience += Time.deltaTime * 10;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    private void CalculateBugs()
    {
        float amt = (TimerDevelop / (1 + (CurrentSkillLevel * 0.2f))) * 2;
        TotalBugs += Mathf.RoundToInt(amt);
        TimerRelease += TimerDevelop;
        TimerDevelop = 0.0f;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    private void UpdateAnimation()
    {
        //change to a random frame in the typing animation
        float frame = Random.Range(0, 6);
        animator.SetFloat("Frame", frame);
        TimerAnimation = Random.Range(0.05f, 0.2f);
    }

    //------------------------------------------------------------------------------------------------------------------------------
    private void Test()
    {
        applicationRenderer.color = testColor;
        if (Bugs > 0)
        {
            spawnController.StartCoroutine("SpawnBugs");
            Testing = true;
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------
    private void Release()
    {
        //code
        TimerRelease = 0;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    public void PlayerLevelUp()
    {
        if (CurrentSkillLevel < 4)
        {
            CurrentSkillLevel++;
            ExperienceToLevel = Mathf.RoundToInt(ExperienceToLevel * 1.2f);
            Experience = 0.0f;
        }
    }
}
