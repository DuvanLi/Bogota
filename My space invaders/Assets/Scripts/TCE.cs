//using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TCE : MonoBehaviour
{
    // INSPECTOR --------------------------------------

    [HideInInspector]
    public int _tabla;
    [HideInInspector]
    public string _gruposTabla;

    [Header("Transición Cambio de Escena")]
    [Space]

    public bool _habilitar = false;
    [Space]

    //[Header("Inicio")]
    public bool _sinInicio;
    public float _tiempoInicio;
    public Material _materialInicio;
    [Space]

    //[Header("Escena")]
    public bool _sinTiempoEscena;
    public float _tiempoEscena;
    public string _escenaSiguiente;
    [Space]

    //[Header("Final")]
    public bool _sinFinal;
    public float _tiempoFinal;
    public Material _materialFinal;

    // PRIVATE ---------------------------------------

    [HideInInspector]
    public bool _pasarEscena;
    [HideInInspector]
    public bool _salirJuego;

    private bool _validarSalida;
    private SpriteRenderer _renderer;

    private float _contadorGeneral = 0;

    private float _tI;
    private float _tF;
    private bool _permitirInicio = true;
    private bool _permitirFinal = true;
    private float _contadorI;
    private float _contadorF;
    private int _fade;


    // ╔════════════════════════════════════ AWAKE ════════════════════════════════════╗

    private void Awake()
    {
        if (_habilitar)
        {
            // Awake Inicio █   
            if (_sinInicio == false)
            {
                TransicionInicio();
            }
            else
            {
                _tiempoInicio = 0;
                _materialInicio = null;
            }

            // Awake Escena █            
            _pasarEscena = false;
            _salirJuego = false;
            _validarSalida = false;

            // Awake Final █ 
            if (_sinFinal)
            {
                _tiempoFinal = 0;
                _materialFinal = null;
            }
        }
    }


    // Awake Inicio █ ─────────────────────────────────────────────────────────────────┐
    private void TransicionInicio()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.material = _materialInicio;

        _renderer.material.SetFloat("_InOut", 1);
        _renderer.material.SetFloat("_Curtain", 1);

        _tI = 0;
        _contadorI = 0;
    }



    // ╔════════════════════════════════════ START ════════════════════════════════════╗

    private void Start()
    {
        _fade = _renderer.material.GetInt("_Fade");

        // Start Escena █
        if (_sinTiempoEscena)
        {
            _tiempoEscena = 0;
        }

        // Start Final █
        if (_sinFinal == false)
        {
            _tF = 0;
            _contadorF = 0;
        }
    }



    // ╔═══════════════════════════════════ UPDATE ════════════════════════════════════╗

    private void Update()
    {
        Debug.Log(_habilitar);

        _contadorGeneral += Time.deltaTime;

        // Update Inicio █
        InterpolacionInicio();

        // Update Escena █
        if (_permitirFinal == false)
        {
            PasarEscena();
        }

        // Update Final █ 
        if (_contadorGeneral >= _tiempoEscena - _tiempoFinal && _sinTiempoEscena == false && _sinFinal == false)
        {
            InterpolacionFinal();
        }

        if (_pasarEscena && _sinTiempoEscena && _sinFinal == false)
        {
            InterpolacionFinal();
        }

        if (_pasarEscena && _sinFinal)
        {
            _permitirFinal = false;
        }

        if (_salirJuego && _sinFinal == false)
        {
            InterpolacionFinal();
        }

        if (_salirJuego && _sinFinal)
        {
            _validarSalida = true;
        }

        if (_validarSalida)
        {
            SalirJuego();
        }
    }


    // Update Inicio █ ────────────────────────────────────────────────────────────────┐
    private void InterpolacionInicio()
    {
        if (!_sinInicio && _permitirInicio && _fade == 1)
        {
            if (_tI < 1)
            {
                float _linealInicio = Mathf.Lerp(1, 0, _tI);

                _renderer.material.SetFloat("_InOut", _linealInicio);

                _contadorI += Time.deltaTime;
                _tI = _contadorI / _tiempoInicio;
            }
            else
            {
                //_renderer.material = null;
                _permitirInicio = false;
            }
        }

        else if (_sinInicio == false && _permitirInicio && _fade == 0)
        {
            if (_tI < 1)
            {
                float _linealInicio = Mathf.Lerp(1, 0, _tI);

                _renderer.material.SetFloat("_Curtain", _linealInicio);

                _contadorI += Time.deltaTime;
                _tI = _contadorI / _tiempoInicio;
            }
            else
            {
                // _renderer.material = null;
                _permitirInicio = false;
            }
        }
    }

    // Update Escena █ ────────────────────────────────────────────────────────────────┐
    private void PasarEscena()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_escenaSiguiente);
    }

    private void SalirJuego()
    {
        Application.Quit();
    }


    // Update Final █ ────────────────────────────────────────────────────────────────┐
    private void InterpolacionFinal()
    {
        if (_permitirFinal && _sinFinal == false)
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.material = _materialFinal;
            _renderer.material.SetFloat("_InOut", 0);
            _renderer.material.SetFloat("_Curtain", 0);

            _fade = _renderer.material.GetInt("_Fade");

            if (_fade == 1)
            {
                if (_tF < 1)
                {
                    float _linealFinal = Mathf.Lerp(0, 1, _tF);

                    _renderer.material.SetFloat("_InOut", _linealFinal);

                    _contadorF += Time.deltaTime;
                    _tF = _contadorF / _tiempoFinal;
                }
                else
                {
                    _renderer.material.SetFloat("_InOut", 1);
                    _permitirFinal = false;
                }
            }

            else
            {
                if (_tF < 1)
                {
                    float _linealFinal = Mathf.Lerp(0, 1, _tF);

                    _renderer.material.SetFloat("_Curtain", _linealFinal);

                    _contadorF += Time.deltaTime;
                    _tF = _contadorF / _tiempoFinal;
                }

                else
                {
                    _renderer.material.SetFloat("_Curtain", 1);
                    _permitirFinal = false;
                }
            }

        }
    }


}