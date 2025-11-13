using Unity.VisualScripting;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private int walkingHash;
    private int runHash;
    void Start()
    {
        _animator = GetComponent<Animator>();
        walkingHash = Animator.StringToHash("walk");
        runHash = Animator.StringToHash("run");
    }

    // Update is called once per frame
    void Update()
    {
        bool run = _animator.GetBool(runHash);
        bool walk = _animator.GetBool(walkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        if (!walk && forwardPressed)
        {
            _animator.SetBool(walkingHash , true);
        }

        if (walk && !forwardPressed)
        {
            _animator.SetBool(walkingHash,false);
        }

        if (!run && (forwardPressed && runPressed))
        {
            _animator.SetBool(runHash,true);
        } 
        
        
        if (run  &&  (!forwardPressed || !runPressed))
        {
            _animator.SetBool(runHash,false);
        }

        
    }
}
