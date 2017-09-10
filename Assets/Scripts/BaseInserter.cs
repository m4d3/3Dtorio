using UnityEngine;

public class BaseInserter : Building
{
    private Animation _anim;

    private readonly int _dir = 0;
    private BaseProduct _product;
    public Transform AttachTransform;
    private Vector3 dropPosition;
    private BaseProduct lastProduct;
    private BoxCollider DropZone;

    private void Awake()
    {
        _anim = GetComponent<Animation>();

    }

    // Use this for initialization
    private void Start()
    {
      
        var grid = FindObjectOfType<Grid>();
    }

    public override void Build()
    {
        dropPosition = transform.Find("DropZone").position;
        //GetComponent<BoxCollider>().center = VectorFromDir() + Vector3.up * 0.1f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_anim.isPlaying)
        {
            if (_product && _product.OnInserter){

                _product.transform.parent = null;
                _product.transform.position = dropPosition;
                _anim["Inserter"].speed = -1;
                _anim["Inserter"].time = _anim["Inserter"].length;
                _anim.Play();
                _product.GetComponent<Collider>().enabled = true;
                _product.OnInserter = null;
                _product = null;
            }
        }
        if (lastProduct && _anim["Inserter"].speed > 0)
        {
            if (lastProduct.transform.position.Equals(dropPosition))
                _anim["Inserter"].speed = 0;
            else
            {
                _anim["Inserter"].speed = 1;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!this.enabled || _product) return;

        if (other.gameObject.GetComponent<BaseProduct>() && !_anim.isPlaying)
        {
            other.GetComponent<Collider>().enabled = false;
            other.transform.position = AttachTransform.position;
            other.transform.parent = AttachTransform;
            _product = other.gameObject.GetComponent<BaseProduct>();
            _product.OnBelt = null;
            _product.OnInserter = this;
            _anim["Inserter"].speed = 1;
            _anim["Inserter"].time = 0;
            _anim.Play();

            lastProduct = _product;
        }
    }
}