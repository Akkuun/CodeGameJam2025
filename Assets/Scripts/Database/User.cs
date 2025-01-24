public class User
{
    public string m_name;
    public string m_id;
    public int m_score;
    public bool m_note1;
    public bool m_note2;
    public bool m_note3;

    public User(string _name, string _id, int _score, bool _note1, bool _note2, bool _note3)
    {
        m_name = _name;
        m_score = _score;
        m_note1 = _note1;
        m_note2 = _note2;
        m_note3 = _note3;
        m_id = _id;
    }

    public string Name
    {
        get { return m_name; }
    }

    public string Id
    {
        get { return m_id; }
    }

    public int Score
    {
        get { return m_score; }
    }

    public bool Note1
    {
        get { return m_note1; }
    }

    public bool Note2
    {
        get { return m_note2; }
    }

    public bool Note3
    {
        get { return m_note3; }
    }
}
