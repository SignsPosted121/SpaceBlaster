using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets
{
	[RequireComponent(typeof(PlayerSounds))]
	public class Player : Ship
	{
		private float Moving { get; set; }
		private float RotatingX { get; set; }
		private float RotatingY { get; set; }
		private float RotatingZ { get; set; }

		public float MouseSensitivity = 1;

		private PlayerSounds sounds;

		public void Accelerate(InputAction.CallbackContext ctx)
		{
			Moving = ctx.ReadValue<float>();
		}

		public void Strafe(InputAction.CallbackContext ctx)
		{
			Strafe(ctx.ReadValue<Vector2>());
		}

		public void Boost(InputAction.CallbackContext ctx)
		{
			if (ctx.performed)
				ActivateBoost(true);
			else if (ctx.canceled)
				ActivateBoost(false);
		}

		public void Turn(InputAction.CallbackContext ctx)
		{
			RotatingY = ctx.ReadValue<float>() * MouseSensitivity;
		}

		public void Dive(InputAction.CallbackContext ctx)
		{
			RotatingX = ctx.ReadValue<float>() * MouseSensitivity;
		}

		public void Twist(InputAction.CallbackContext ctx)
		{
			RotatingZ = ctx.ReadValue<float>();
		}

		public void LockMouse(InputAction.CallbackContext ctx)
		{
			if (ctx.performed)
				Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public override bool CheckIfColliding()
		{
			var result = base.CheckIfColliding();
			if (result)
				sounds.PlaySound("Explosion");
			return result;
		}

		private void OnDestroy()
		{
			Cursor.lockState = CursorLockMode.None;
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			sounds = GetComponent<PlayerSounds>();
		}

		protected override void Update()
		{
			Vector3 rotation = new Vector3(RotatingX, RotatingY, RotatingZ);

			Move(Moving);
			Rotate(rotation);

			base.Update();

			CameraScript.SetZoom(CameraScript.GetBaseZoom() + 15 * Velocity / 100);

			CameraScript.SetCameraShake(Mathf.Exp(Velocity / 250));

			var sound = sounds.GetSound("Thrusters");
			sound.pitch = 0.6f + Velocity / MaxVelocity;
		}
	}
}
