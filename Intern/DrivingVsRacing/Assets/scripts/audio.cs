using System;
using UnityEngine;
using Random = UnityEngine.Random;


    public class audio : MonoBehaviour
    {
    // Tập lệnh này đọc một số thuộc tính hiện tại của ô tô và phát âm thanh tương ứng.
    // Âm thanh động cơ có thể là một đoạn đơn đơn giản được lặp lại và cao độ, hoặc
    // có thể là sự pha trộn đan chéo của bốn clip thể hiện âm sắc của động cơ
    // ở các trạng thái RPM và Throttle khác nhau.

    // tất cả các clip động cơ phải là một cao độ ổn định, không tăng hoặc giảm.

    // khi sử dụng giao diện công cụ bốn kênh, bốn clip phải là:
    // lowAccelClip: Động cơ ở số vòng quay thấp, với bướm ga mở (tức là bắt đầu tăng tốc ở tốc độ rất thấp)
    // highAccelClip: Thenengine ở số vòng quay cao, với bướm ga mở (tức là tăng tốc, nhưng gần như ở tốc độ tối đa)
    // lowDecelClip: Động cơ ở số vòng quay thấp, với ga ở mức tối thiểu (tức là chạy không tải hoặc phanh động cơ ở tốc độ rất thấp)
    // highDecelClip: Động cơ ở số vòng quay cao, với ga ở mức tối thiểu (tức là phanh động cơ ở tốc độ rất cao)

    // Để ghép chéo thích hợp, tất cả các cao độ của clip phải khớp với nhau, với khoảng cách quãng tám giữa thấp và cao.


    public enum EngineAudioOptions // Tùy chọn cho âm thanh động cơ
    {
            Simple, // Âm thanh kiểu đơn giản
            FourChannel // âm thanh bốn lua chon
    }

        public EngineAudioOptions engineSoundStyle = EngineAudioOptions.FourChannel;
        public AudioClip lowAccelClip;                                              
        public AudioClip lowDecelClip;                                              
        public AudioClip highAccelClip;                                             
        public AudioClip highDecelClip;                                             
        public float pitchMultiplier = 1f;                                          
        public float lowPitchMin = 1f;                                              
        public float lowPitchMax = 6f;                                              
        public float highPitchMultiplier = 0.25f;                                   
        public float maxRolloffDistance = 500;                                      
        public float dopplerLevel = 1;                                              
        public bool useDoppler = true;                                              

        private AudioSource m_LowAccel; 
        private AudioSource m_LowDecel; 
        private AudioSource m_HighAccel; 
        private AudioSource m_HighDecel; 
        private bool m_StartedSound; 
        public controller m_CarController;
        public inputManager InputManager;
        private AIcontroller aicontroler;
        
        private void StartSound()
        {
            
            m_HighAccel = SetUpEngineAudioSource(highAccelClip);

           
            if (engineSoundStyle == EngineAudioOptions.FourChannel)
            {
                m_LowAccel = SetUpEngineAudioSource(lowAccelClip);
                m_LowDecel = SetUpEngineAudioSource(lowDecelClip);
                m_HighDecel = SetUpEngineAudioSource(highDecelClip);
            }

            
            m_StartedSound = true;
        }


        private void StopSound()
        {
            
            foreach (var source in GetComponents<AudioSource>())
            {
                Destroy(source);
            }

            m_StartedSound = false;
        }


   
    private void FixedUpdate()
        {
            
            float camDist = (Camera.main.transform.position - transform.position).sqrMagnitude;

            
            if (m_StartedSound && camDist > maxRolloffDistance*maxRolloffDistance)
            {
                StopSound();
            }

            
            if (!m_StartedSound && camDist < maxRolloffDistance*maxRolloffDistance)
            {
                StartSound();
            }

            if (m_StartedSound)
            {
                
                float pitch = ULerp(lowPitchMin, lowPitchMax, m_CarController.engineRPM / m_CarController.maxRPM);

                
                pitch = Mathf.Min(lowPitchMax, pitch);

                if (engineSoundStyle == EngineAudioOptions.Simple)
                {
                    
                    m_HighAccel.pitch = pitch*pitchMultiplier*highPitchMultiplier;
                    m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                    m_HighAccel.volume = 1;
                }
                else
                {
                    
                    m_LowAccel.pitch = pitch*pitchMultiplier;
                    m_LowDecel.pitch = pitch*pitchMultiplier;
                    m_HighAccel.pitch = pitch*highPitchMultiplier*pitchMultiplier;
                    m_HighDecel.pitch = pitch*highPitchMultiplier*pitchMultiplier;
                    float accFade = 0;
                
       
                    accFade = Mathf.Abs((InputManager.vertical > 0 && !m_CarController.test ) ?InputManager.vertical : 0);

                    float decFade = 1 - accFade;

                    
                    float highFade = Mathf.InverseLerp(0.2f, 0.8f,  m_CarController.engineRPM / 10000);
                    float lowFade = 1 - highFade;

                    
                    highFade = 1 - ((1 - highFade)*(1 - highFade));
                    lowFade = 1 - ((1 - lowFade)*(1 - lowFade));
                    accFade = 1 - ((1 - accFade)*(1 - accFade));
                    decFade = 1 - ((1 - decFade)*(1 - decFade));

                    
                    m_LowAccel.volume = lowFade*accFade;
                    m_LowDecel.volume = lowFade*decFade;
                    m_HighAccel.volume = highFade*accFade;
                    m_HighDecel.volume = highFade*decFade;

                    
                    m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                    m_LowAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                    m_HighDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                    m_LowDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                }
            }
        }


        
        private AudioSource SetUpEngineAudioSource(AudioClip clip)
        {
            
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = 0;
            source.spatialBlend = 1;
            source.loop = true;

            
            source.time = Random.Range(0f, clip.length);
            source.Play();
            source.minDistance = 5;
            source.maxDistance = maxRolloffDistance;
            source.dopplerLevel = 0;
            return source;
        }


        
        private static float ULerp(float from, float to, float value)
        {
            return (1.0f - value)*from + value*to;
        }
    }
