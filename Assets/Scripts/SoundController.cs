using UnityEngine;
using System.Collections;
using System.Linq;

public class SoundController : MonoBehaviour {

	[SerializeField] public AudioClip[] _audioClips;		//Набор всех аудиоклипов используемых в приложении, также сохраняется возможнось проиграть клип напрямую
	int _audioChannels = 5;		//Максимальное количество звуковых каналов
	float _masterVol = .5f;			//Стандартная громкость всех проигрываемыз звуков
	float _soundVol = 1f;			//Volume multiplier for sound effects
	float _musicVol = 1f;			//Volume multiplier for music
	public bool _linearRollOff = false;			//Enable to change rollOff
	AudioSource[] _channels;			//Звуковые каналы
	int _channel;					//Текущий канал
	AudioSource[] _musicChannels;	//Музыкальные каналы
	int _musicChannel;				//Текущий канал
	private float _currentMusicVol;		//Cache for the music clips volume, makes controller able to change volume on runtime. Run UpdateMusicVolume(); after changing _musicVol
	private float _fadeTo;				//Music will fade to this value in FadeUpMusic()
	public static SoundController instance;	// SoundController is a singleton. SoundController.instance.DoSomeThing();

	void OnApplicationQuit() {			// Ensure that the instance is destroyed when the game is stopped in the editor.
		instance = null;
	}

	void Start () {
		if (instance){
			Destroy (gameObject);
		}else{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		AddChannels();
		
		DontDestroyOnLoad (transform.gameObject);

		LoadClips();
	}
	
	void LoadClips(){
		_audioClips = (AudioClip[]) Resources.LoadAll("Sounds", typeof(AudioClip)).Cast<AudioClip>().ToArray();
		Debug.Log("Loaded sounds: " + _audioClips.Length);
	}
	
	public void StopMusic(bool fade) {
		PlayMusic(null, 0f, 1f, fade);
	}
	
	void FadeUpMusic(){
		if(_musicChannels[_musicChannel].volume < _fadeTo){
			_musicChannels[_musicChannel].volume += 0.0025f;	
		}else{
			
			CancelInvoke("FadeUpMusic");
		}
	}
	
	void FadeDownMusic(){
		int c = 0;
		if(_musicChannel == 0)
			c = 1;
		if(_musicChannels[c].volume > 0f){
			_musicChannels[c].volume -= 0.0025f;
		}else{
			_musicChannels[c].Stop();
			CancelInvoke("FadeDownMusic");
		}
	}

	void UpdateMusicVolume(){
		for(int j = 0; j < 2; j++){	
			_musicChannels[j].volume = _currentMusicVol*_masterVol*_musicVol;
		}
	}

	void AddChannels () {
		//Add channels to stage (Future Update Note: decrease startup peak if this is done in editor)
		_channels = new AudioSource[_audioChannels];
		_musicChannels = new AudioSource[2];
		if(_channels.Length <= _audioChannels){		
			for(int i = 0; i < _audioChannels; i++){	
				GameObject chan = new GameObject();
				chan.AddComponent<AudioSource>();
				chan.name = "AudioChannel " + i;
				chan.transform.parent = this.transform;
				_channels[i] = chan.GetComponent<AudioSource>();
				if(_linearRollOff)	
					_channels[i].rolloffMode =  AudioRolloffMode.Linear;
			}
		}
		for(int j = 0; j < 2; j++){	
			GameObject mchan = new GameObject();		
			mchan.AddComponent<AudioSource>();
			mchan.name = "MusicChannel " + j;
			mchan.transform.parent = this.transform;
			_musicChannels[j] = mchan.GetComponent<AudioSource>();	
			_musicChannels[j].loop = true;
			_musicChannels[j].volume = 0f;
			if(_linearRollOff)
				_musicChannels[j].rolloffMode =  AudioRolloffMode.Linear;
		}
	}

	//Play music clip
	public void PlayMusic (string sndName, float volume, float pitch, bool fade) {
		if(!fade)_musicChannels[_musicChannel].volume = 0f;
		if(_musicChannel == 0) _musicChannel = 1;
		else _musicChannel = 0;
		_currentMusicVol = volume;
		_musicChannels[_musicChannel].clip = GetClipByName(sndName);;
		if(fade){
			this._fadeTo = volume*_masterVol*_musicVol;
			InvokeRepeating("FadeUpMusic", 0.01f, 0.01f);
			InvokeRepeating("FadeDownMusic", 0.01f, 0.01f);
		}else{
			_musicChannels[_musicChannel].volume = volume*_masterVol*_musicVol;
		}
		_musicChannels[_musicChannel].GetComponent<AudioSource>().pitch = pitch;
		_musicChannels[_musicChannel].GetComponent<AudioSource>().Play();
	}

	//Play by name
	public void Play (string sndName, float volume, float pitch) {
		if(_channel < _channels.Length-1)	_channel++;
		else _channel = 0;	
		_channels[_channel].clip = GetClipByName(sndName);
		_channels[_channel].GetComponent<AudioSource>().volume = volume*_masterVol*_soundVol;
		_channels[_channel].GetComponent<AudioSource>().pitch = pitch;
		_channels[_channel].transform.position = Vector3.zero;
		_channels[_channel].GetComponent<AudioSource>().Play();
		Debug.Log("Play: " + _channels[_channel].clip);
	}

	//Play from channels list
	public void Play (int audioClipIndex, float volume, float pitch) {
		if(_channel < _channels.Length-1) _channel++;
		else _channel = 0;
		if(audioClipIndex < _audioClips.Length){	
			_channels[_channel].clip = _audioClips[audioClipIndex];
			_channels[_channel].GetComponent<AudioSource>().volume = volume*_masterVol*_soundVol;
			_channels[_channel].GetComponent<AudioSource>().pitch = pitch;
			_channels[_channel].GetComponent<AudioSource>().Play();
		}
	}

	//Play clip
	public void Play (AudioClip clip, float volume, float pitch, Vector3 position) {
		if(_channel < _channels.Length-1)	_channel++;
		else _channel = 0;	
		_channels[_channel].clip = clip;
		_channels[_channel].GetComponent<AudioSource>().volume = volume*_masterVol*_soundVol;
		_channels[_channel].GetComponent<AudioSource>().pitch = pitch;
		_channels[_channel].transform.position = position;
		_channels[_channel].GetComponent<AudioSource>().Play();
	}

	AudioClip GetClipByName(string sndName){
		foreach(AudioClip clip in _audioClips){
			if (clip.name == sndName) {
				return clip;
			}
		}

		Debug.Log("Clip [" + sndName + "] not found!");
		return null;
	}
	
	void StopAll () {	//Stops all sound from channels (Future Update Note: activate delay?)
		for(int i = 0; i < _channels.Length; i++){
			_channels[i].Stop();	
		}
	}

}