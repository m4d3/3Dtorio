using UnityEngine;

public class BaseInserter : Building
{
    private Animation _anim;

    private readonly int _dir = 0;
    private BaseProduct _product;
    public Transform AttachTransform;
    private Tile entryTile;
    private Tile exitTile;

    private void Awake()
    {
        _anim = GetComponent<Animation>();
    }

    // Use this for initialization
    private void Start()
    {
      
        var grid = FindObjectOfType<Grid>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_anim.isPlaying && _product && _product.OnInserter)
        {
            _product.transform.parent = null;
            _anim["Inserter"].speed = -1;
            _anim["Inserter"].time = _anim["Inserter"].length;
            _anim.Play();
            _product.OnInserter = null;
            _product = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!this.enabled) return;

        if (other.gameObject.GetComponent<BaseProduct>()
            && !_anim.isPlaying
            && !_product)
        {
            other.transform.position = AttachTransform.position;
            other.transform.parent = AttachTransform;
            _product = other.gameObject.GetComponent<BaseProduct>();
            _product.OnBelt = null;
            _product.OnInserter = this;
            _anim.Play();
        }
    }
}