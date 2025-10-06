    using UnityEngine;

    public class VelocityAnimationBlender : MonoBehaviour
    {
        public AnimationClip idleAnimation;
        public AnimationClip walkAnimation;
        //public AnimationClip runAnimation;
        public float walkSpeedThreshold = 1.0f;
        public float runSpeedThreshold = 3.0f;
        public float blendSpeed = 5.0f; // Speed of blending between animations

        public Animation _animationComponent;
        public Rigidbody _rigidbody; // Or CharacterController, depending on your setup

        void Start()
        {
            _animationComponent = GetComponent<Animation>();
            _rigidbody = GetComponent<Rigidbody>(); // Or GetComponent<CharacterController>();

            // Ensure animations are assigned and added to the Animation component
            if (idleAnimation != null) _animationComponent.AddClip(idleAnimation, idleAnimation.name);
            if (walkAnimation != null) _animationComponent.AddClip(walkAnimation, walkAnimation.name);
            //if (runAnimation != null) _animationComponent.AddClip(runAnimation, runAnimation.name);

            // Play idle initially
            if (idleAnimation != null) _animationComponent.Play(idleAnimation.name);
        }

        void Update()
        {
            float currentSpeed = 0f;
            if (_rigidbody != null)
            {
                currentSpeed = _rigidbody.linearVelocity.magnitude;
            }
            // If using CharacterController
            // else if (_characterController != null)
            // {
            //     currentSpeed = _characterController.velocity.magnitude;
            // }

            if (currentSpeed < walkSpeedThreshold)
            {
                // Blend towards idle
                if (idleAnimation != null) _animationComponent.CrossFade(idleAnimation.name, blendSpeed);
            }
            else if (currentSpeed >= walkSpeedThreshold && currentSpeed < runSpeedThreshold)
            {
                // Blend towards walk
                if (walkAnimation != null) _animationComponent.CrossFade(walkAnimation.name, blendSpeed);
            }
            //else if (currentSpeed >= runSpeedThreshold)
            {
                // Blend towards run
                //if (runAnimation != null) _animationComponent.CrossFade(runAnimation.name, blendSpeed);
            }
        }
    }