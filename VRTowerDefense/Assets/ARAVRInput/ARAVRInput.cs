﻿#define PC
//#define Oculus
//#define Vive

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

#if Vive
using Valve.VR;
#endif

public static class ARAVRInput
{
#if PC
    public static bool isPC = true;
#else
    public static bool isPC = false;
    static Transform rootTransform;

#endif

#if PC
    public enum ButtonTarget
    {
        Fire1,
        Fire2,
        Fire3,
    }
#elif Vive
    public enum ButtonTarget
    {
        Teleport,
        InteractUI,
        GrabGrip,
        Jump,
    }
#endif

    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Fire2,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2
#elif Oculus
        One = OVRInput.Button.One,
        Two = OVRInput.Button.Two,
        Thumbstick = OVRInput.Button.PrimaryThumbstick,
        IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
        HandTrigger = OVRInput.Button.PrimaryHandTrigger
#elif Vive
        One = ButtonTarget.InteractUI,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Teleport,
        IndexTrigger = ButtonTarget.InteractUI,
        HandTrigger = ButtonTarget.GrabGrip,
#endif
    }

    public enum Controller
    {
#if PC
        LTouch,
        RTouch
#elif Oculus
        LTouch = OVRInput.Controller.LTouch,
        RTouch = OVRInput.Controller.RTouch
#elif Vive
        LTouch = SteamVR_Input_Sources.LeftHand,
        RTouch = SteamVR_Input_Sources.RightHand,
#endif
    }


#if Oculus
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {
            rootTransform = GameObject.Find("TrackingSpace").transform;
        }

        return rootTransform;
    }
#elif Vive
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {
            rootTransform = GameObject.Find("[CameraRig]").transform;
        }

        return rootTransform;
    }
#endif

    // 오른쪽 컨트롤러의 위치 얻어오기
    public static Vector3 RHandPosition
    {
        get
        {
#if PC
            /* 같은 결과를 볼 수 있다.
            Plane plane = new Plane(Vector3.up, 0);
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(r, out distance))
            {
                return r.GetPoint(distance);
            }
            */
            // 마우스의 스크린좌표 얻어오기
            Vector3 pos = Input.mousePosition;
            // z 값은 카메라의 near 값으로 할당
            pos.z = Camera.main.nearClipPlane;
            // 스크린 좌표를 월드좌표로 변환
            pos = Camera.main.ScreenToWorldPoint(pos);

            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#elif Vive
            Vector3 pos = RHand.position;
            return pos;
#endif
        }
    }

    // 오른쪽 컨트롤러의 방향 얻어오기
    public static Vector3 RHandDirection
    {
        get
        {
#if PC
            Vector3 direction = RHandPosition - Camera.main.transform.position;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);
            return direction;
#elif Vive
            Vector3 direction = RHand.forward;
            return direction;
#endif
        }
    }

    // 왼쪽 컨트롤러의 위치 얻어오기
    public static Vector3 LHandPosition
    {
        get
        {
#if PC
            /*
            Plane plane = new Plane(Vector3.up, 0);
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(r, out distance))
            {
                return r.GetPoint(distance);

            }
            */
            // 마우스의 스크린좌표 얻어오기
            Vector3 pos = Input.mousePosition;
            // z 값은 카메라의 near 값으로 할당
            pos.z = Camera.main.nearClipPlane;
            // 스크린 좌표를 월드좌표로 변환
            pos = Camera.main.ScreenToWorldPoint(pos);
            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#elif Vive
            Vector3 pos = LHand.position;
            return pos;
#endif
        }
    }

    // 왼쪽 컨트롤러의 방향얻어오기
    public static Vector3 LHandDirection
    {
        get
        {
#if PC
            Vector3 direction = LHandPosition - Camera.main.transform.position;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);
            return direction;
#elif Vive
            Vector3 direction = LHand.forward;
            return direction;
#endif
        }
    }

    // 왼쪽 컨트롤러
    static Transform lHand;
    // 오른쪽 컨트롤러
    static Transform rHand;

    // 씬에 등록된 왼쪽 컨트롤러 찾아서 반환
    public static Transform LHand
    {

        get
        {
            // 만약 lHand 에 값이 없을경우
            if (lHand == null)
            {
#if PC
                // LHand 이름으로 게임오브젝트를 만든다.
                GameObject handObj = new GameObject("LHand");
                // 만들어진 객체의 트렌스폼을 lHand 에 할당
                lHand = handObj.transform;
                // 컨트롤러의 위치값으로 lHand 객체의 위치를 할당
                lHand.position = LHandPosition;
                // 컨트롤러가 향하는 방향을 LHandDirection 으로 할당
                lHand.forward = LHandDirection;
                // 컨트롤러를 카메라의 자식 객체로 등록
                lHand.parent = Camera.main.transform;
#elif Oculus
                lHand = GameObject.Find("LeftControllerAnchor").transform;
#elif Vive
                lHand = GameObject.Find("Controller (left)").transform;
#endif
            }
            return lHand;
        }

    }
    // 씬에 등록된 오른쪽 컨트롤러 찾아서 반환
    public static Transform RHand
    {

        get
        {
            // 만약 rHand 에 값이 없을경우
            if (rHand == null)
            {
#if PC
                // RHand 이름으로 게임오브젝트를 만든다.
                GameObject handObj = new GameObject("RHand");
                // 만들어진 객체의 트렌스폼을 rHand 에 할당
                rHand = handObj.transform;
                // 컨트롤러의 위치값으로 rHand 객체의 위치를 할당
                rHand.position = RHandPosition;
                // 컨트롤러가 향하는 방향을 RHandPosition 으로 할당
                rHand.forward = RHandDirection;
                // 컨트롤러를 카메라의 자식 객체로 등록
                rHand.parent = Camera.main.transform;
#elif Oculus
                rHand = GameObject.Find("RightControllerAnchor").transform;
#elif Vive
                rHand = GameObject.Find("Controller (right)").transform;
#endif
            }
            return rHand;
        }
    }

    // 컨트롤러의 특정 버튼을 누르고 있는 동안 true 를 반환
    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        // virtualMask 에 들어온 값을 ButtonTarget 타입으로 변환하여 전달한다.
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        //return SteamVR_Actions._default.Teleport.state;
        return SteamVR_Input.GetState(((ButtonTarget)virtualMask).ToString(), (SteamVR_Input_Sources)(hand));
#endif
    }

    // 컨트롤러의 특정 버튼을 눌렀을 때 true 를 반환
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        //return SteamVR_Actions._default.Teleport.stateDown;
        return SteamVR_Input.GetStateDown(((ButtonTarget)virtualMask).ToString(), (SteamVR_Input_Sources)(hand));
#endif
    }
    // 컨트롤러의 특정 버튼을 눌렀다 떼었을 때 true 를 반환

    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        //return SteamVR_Actions._default.Teleport.stateUp;
        return SteamVR_Input.GetStateUp(((ButtonTarget)virtualMask).ToString(), (SteamVR_Input_Sources)(hand));
#endif
    }

    // 컨트롤러의 Axis 입력을 반환
    // axis : Horizontal, Vertical 값을 갖는다.
    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#elif Oculus
        if (axis == "Horizontal")
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).x;
        }
        else
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).y;
        }
#elif Vive
        if (axis == "Horizontal")
        {
            return SteamVR_Input.GetVector2("TouchPad", (SteamVR_Input_Sources)(hand)).x;
        }
        else
        {
            return SteamVR_Input.GetVector2("TouchPad", (SteamVR_Input_Sources)(hand)).y;
        }
#endif
    }

    // 컨트롤러에 진동 호출 하기
    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibration(0.06f, 1, 1, hand);
#elif Vive
        PlayVibration(0.06f, 160, 0.5f, hand);
#endif
    }

    // 컨트롤러에 진동 호출 하기
    // waitTime : 지속시간, duration : 반복횟수(시간), frequency : 빈도, amplify : 진폭, hand : 왼쪽 혹은 오른쪽 컨트롤러
    public static void PlayVibration(float duration, float frequency, float amplitude, Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance == null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#elif Vive
        SteamVR_Actions._default.Haptic.Execute(0, duration, frequency, amplitude, (SteamVR_Input_Sources)hand);
#endif
    }

#if Oculus
    static IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude, Controller hand)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, (OVRInput.Controller)hand);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)hand);
    }
#endif

    // 카메라가 바라보는 방향을 기준으로 센터를 잡는다.
    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#elif Vive
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.TrackingReference);
            subsystems[i].TryRecenter();
        }

#endif
    }

    // 원하는 방향으로 타겟의 센터를 설정
    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }

#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif

    // 광선 레이가 닿는 곳에 크로스헤어를 위치시키고 싶다.
    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller hand = Controller.RTouch)
    {
        // 1. 광선 레이 만들기
        Ray ray;
        // 컨트롤러의 위치와 방향을 이용하여 Ray 제작
        if (isHand)
        {
#if PC
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            if (hand == Controller.RTouch)
            {
                ray = new Ray(RHandPosition, RHandDirection);
            }
            else
            {
                ray = new Ray(LHandPosition, LHandDirection);
            }
#endif
        }
        else
        {
            // 카메라 기준으로 화면의 정 중앙으로 Ray 를 제작
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        // 눈에 안보이는 Plane 을 만든다.
        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        // plane 을 이용하여 ray 를 쏜다.
        if (plane.Raycast(ray, out distance))
        {
            // ray 의 GetPoint 함수를 이용하여 충돌 지점의 위치를 가져온다.
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;
            // 크로스헤어의 크기를 최소 기본 크기에서 거리에 따라 더 커지도록 한다.
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
        else
        {
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -Camera.main.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
    }
}

// ARAVRInput 클래에서 사용할 코루틴 객체
class CoroutineInstance : MonoBehaviour
{
    public static CoroutineInstance coroutineInstance = null;
    private void Awake()
    {
        if (coroutineInstance == null)
        {
            coroutineInstance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
