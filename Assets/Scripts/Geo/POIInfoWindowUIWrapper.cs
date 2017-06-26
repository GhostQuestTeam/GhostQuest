using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POIInfoWindowUIWrapper : MonoBehaviour
{

    private GameObject _baseObj;

    private Text _txtPointOwner;
    private Button _btnClose;
    private Button _btnToFight;
    private Text _txtInfo;

    public GameObject BaseObject
    {
        get
        {
            return _baseObj;
        }
        set
        {
            _baseObj = value;
            bind();
        }
    }

    public string PointOwner
    {
        get
        {
            return _txtPointOwner.text;
        }
        set
        {
            _txtPointOwner.text = value;
        }
    }

    public string Info
    {
        get
        {
            return _txtInfo.text;
        }
        set
        {
            _txtInfo.text = value;
        }
    }

    public Button Close
    {
        get
        {
            return _btnClose;
        }
    }

    public Button ToFight
    {
        get
        {
            return _btnToFight;
        }
    }

    public class GhostPanel
    {
        public GameObject _baseObj;
        private Text _txtInfo;
        private string _sType;
        private int _nNum;

        public GhostPanel(GameObject baseObj = null)
        {
            _baseObj = baseObj;
            bind();
        }

        public bool bind()
        {
            if (_baseObj != null)
                _txtInfo = _baseObj.transform.Find("Text").gameObject.GetComponent<Text>();
            return (_baseObj == null) || (_txtInfo == null);
        }

        public string stringify()
        {
            return _sType + " : " + _nNum.ToString();
        }

        public void updateUI()
        {
            _txtInfo.text = stringify();
        }

        public string Type
        {
            get { return _sType; }
            set
            {
                _sType = value;
                updateUI();
            }
        }

        public int Num
        {
            get { return _nNum; }
            set
            {
                _nNum = value;
                updateUI();
            }
        }

    }

    public class Ghosts
    {
        public GameObject _baseObj;
        private int _nMaxSize;
        private int _nCurSize;
        private Dictionary<string, GhostPanel> _panels = new Dictionary<string, GhostPanel>();
        private List<GameObject> _freePanelsPool = new List<GameObject>();

        public Ghosts(GameObject baseObj = null)
        {
            _baseObj = baseObj;
            _nCurSize = 0;
            bind();
        }

        public bool bind()
        {
            _freePanelsPool.Clear();
            int childCount = _baseObj.transform.childCount;
            _nMaxSize = childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject curChild = _baseObj.transform.GetChild(i).gameObject;
                curChild.SetActive(false);
                _freePanelsPool.Add(curChild);
            }
            return _baseObj == null;
        }

        public bool add(string ghostType = "", int ghostNum = 0)
        {
            if (!_panels.ContainsKey(ghostType) && _freePanelsPool.Capacity > 0 && _nCurSize <= _nMaxSize)
            {
                GameObject panel = _freePanelsPool[0];
                _freePanelsPool.RemoveAt(0);
                panel.SetActive(true);
                GhostPanel ghostPanel = new GhostPanel(panel);
                ghostPanel.Type = ghostType;
                ghostPanel.Num = ghostNum;
                _panels.Add(ghostType, ghostPanel);

                _nCurSize++;
                return true;
            }
            return false;
        }

        public bool setGhostNum(string ghostType = "", int ghostNum = 0)
        {
            if (_panels.ContainsKey(ghostType))
            {
                _panels[ghostType].Num = ghostNum;
                return true;
            }
            return false;
        }

        public bool remove(string ghostType = "")
        {
            if (_panels.ContainsKey(ghostType))
            {
                GhostPanel ghostPanel = _panels[ghostType];
                GameObject panel = ghostPanel._baseObj;

                _panels.Remove(ghostType);
                panel.SetActive(false);
                _freePanelsPool.Add(panel);

                _nCurSize--;
                return true;
            }
            return false;
        }

    }

    private Ghosts _ghosts;

    public Ghosts ghosts
    {
        get
        {
            return _ghosts;
        }
    }

    public bool bind()
    {
        if (_baseObj == null)
            return false;
        try
        {
            _txtPointOwner = _baseObj.transform.Find("HeadPanel/PointOwner/Text").gameObject.GetComponent<Text>();
            _txtInfo = _baseObj.transform.Find("InfoPanel/Text").gameObject.GetComponent<Text>();
            _btnClose = _baseObj.transform.Find("HeadPanel/CloseButton").gameObject.GetComponent<Button>();
            _btnToFight = _baseObj.transform.Find("ToFightPanel/ToFightButton").gameObject.GetComponent<Button>();
            GameObject ghostsObj = _baseObj.transform.Find("GhostsPanel/Ghosts").gameObject;
            _ghosts = new Ghosts(ghostsObj);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        return false;
    }

    public POIInfoWindowUIWrapper() { }


    public static bool operator ==(POIInfoWindowUIWrapper r1, POIInfoWindowUIWrapper r2)
    {
        if (object.ReferenceEquals(r1, r2))
        {
            // handles if both are null as well as object identity
            return true;
        }

        if ((object)r1 == null || (object)r2 == null)
        {
            return false;
        }

        return (r1.BaseObject == r2.BaseObject);


    }

    public static bool operator !=(POIInfoWindowUIWrapper r1, POIInfoWindowUIWrapper r2)
    {
        return !(r1 == r2);
    }

}
