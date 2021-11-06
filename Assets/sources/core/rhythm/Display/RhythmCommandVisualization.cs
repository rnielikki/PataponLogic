using UnityEngine;
using UnityEngine.UI;
using PataRoad.Core.Rhythm.Command;
using System.Collections.Generic;

namespace PataRoad.Core.Rhythm.Display
{
    public class RhythmCommandVisualization : MonoBehaviour
    {
        // Start is called before the first frame update
        Image _image;
        private Dictionary<CommandSong, Sprite> _sprites;
        private const string _sourcePath = "Rhythm/Images/Visual/";
        void Awake()
        {
            _image = GetComponent<Image>();
            _sprites = new Dictionary<CommandSong, Sprite>()
            {
                { CommandSong.Patapata, Resources.Load<Sprite>(_sourcePath + "patapata")},
                { CommandSong.Ponpon, Resources.Load<Sprite>(_sourcePath + "ponpon")},
                { CommandSong.Chakachaka, Resources.Load <Sprite>(_sourcePath + "chakachaka")},
                { CommandSong.Ponpata, Resources.Load <Sprite>(_sourcePath + "ponpata")},
                { CommandSong.Ponchaka, Resources.Load<Sprite>(_sourcePath + "ponchaka")},
                { CommandSong.Dondon, Resources.Load<Sprite>(_sourcePath + "dondon")},
                { CommandSong.Donchaka, Resources.Load<Sprite>(_sourcePath + "donchaka")},
                { CommandSong.Patachaka, Resources.Load<Sprite>(_sourcePath + "patachaka")}
            };
            Hide();
        }
        public void SetImage(RhythmCommandModel command)
        {
            if (_sprites.TryGetValue(command.Song, out Sprite sprite))
            {
                if (!_image.enabled) Show();
                _image.sprite = sprite;
            }
            else
            {
                Hide();
            }
        }
        private void Show() =>
            _image.enabled = true;
        public void Hide() =>
            _image.enabled = false;
    }
}
