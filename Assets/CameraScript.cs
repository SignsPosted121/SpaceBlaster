using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
	public class CameraScript : MonoBehaviour
	{
		public static CameraScript singleton;

		public Camera Cam;
		public Transform Player;
		public Vector3 CameraOffset;
		public Vector3 CameraAngle;

		public Vector3 RandomCameraOffset;

		private float CameraShakeIntensity;
		private float CameraZoom;
		private float CameraBaseZoom;

		public static void SetCameraOffset(Vector3 CameraOffset)
		{
			singleton.CameraOffset = CameraOffset;
		}

		public static void SetCameraAngle(Vector3 CameraAngle)
		{
			singleton.CameraAngle = CameraAngle;
		}

		public static void SetCameraShake(float intensity)
		{
			singleton.CameraShakeIntensity = intensity;
		}

		public static void SetZoom(float zoom)
		{
			singleton.CameraZoom = zoom;
		}

		public static float GetBaseZoom()
		{
			return singleton.CameraBaseZoom;
		}

		private static Vector3 GetRandomCameraOffset(float intensity)
		{
			intensity = Mathf.Abs(intensity);
			float x = UnityEngine.Random.Range(-intensity, intensity);
			float y = UnityEngine.Random.Range(-intensity, intensity + 0.1f);
			return new Vector3(x, y);
		}

		private void Awake()
		{
			if (singleton != null)
			{
				enabled = false;
				return;
			}
			singleton = this;
			Cam = Camera.main;
			CameraBaseZoom = Cam.fieldOfView;
		}

		private void LateUpdate()
		{
			if (Player == null)
				return;

			Cam.transform.SetPositionAndRotation(Player.position, Player.rotation);

			RandomCameraOffset = Vector3.Lerp(RandomCameraOffset, GetRandomCameraOffset(CameraShakeIntensity), CameraShakeIntensity * Time.deltaTime);

			Cam.fieldOfView = CameraZoom;

			Cam.transform.Translate(CameraOffset + RandomCameraOffset, Space.Self);
			Cam.transform.Rotate(CameraAngle, Space.Self);
		}
	}
}
