mergeInto(LibraryManager.library, {

  GetDeviceExtern: function () {
    var device = ysdk.deviceInfo.type;
    var bufferSize = lengthBytesUTF8(device) + 1;
    var buffer = _malloc(bufferSize);
    console.log('Detected device type:', device);
    stringToUTF8(device, buffer, bufferSize);
    return buffer;
  },

  GetLanguageExtern: function () {
    var language = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(language) + 1;
    var buffer = _malloc(bufferSize);
    console.log('Detected language:', language);
    stringToUTF8(language, buffer, bufferSize);
    return buffer;
  },

  SaveDataExtern: function (data) {
    var dataString = UTF8ToString(data);
    var dataObj = JSON.parse(dataString);
    player.setData(dataObj).then(() => {
      console.log('Data saved.');
      gameInstance.SendMessage('YandexSDK', 'OnDataSaved');
    });
  },

  LoadDataExtern: function () {
    player.getData().then(data => {
      var dataString = JSON.stringify(data);
      console.log('Data loaded.');
      gameInstance.SendMessage('YandexSDK', 'OnDataLoaded', dataString);
    });
  },

  SetLeaderboardExtern: function (boardID, value) {
    ysdk.isAvailableMethod('leaderboards.setLeaderboardScore').then(result => {
      if (result === true) {
          ysdk.getLeaderboards().then(lb => {
            lb.setLeaderboardScore(UTF8ToString(boardID), value).then(() => {
              console.log('Leaderboard updated.');
            });
        });
      };
    });
  },

  ShowFullscreenAdvExtern: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
          onClose: function(wasShown) {
            console.log('Fullscreen adv closed with result:', wasShown)
          },
          onError: function(error) {
            console.log('Fullscreen adv opened with error:', error);
          }
      }
    });
  },

  ShowRewardedAdvExtern: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
          onOpen: () => {
            console.log('Video adv opened.');
          },
          onRewarded: () => {
            console.log('Video adv rewarded.');
            gameInstance.SendMessage('YandexSDK', 'OnAdvRewarded', 1);
          },
          onClose: () => {
            console.log('Video adv closed.');
            gameInstance.SendMessage('YandexSDK', 'OnAdvRewarded', 0);
          },
          onError: (error) => {
            console.log('Video adv opened with error:', error);
            gameInstance.SendMessage('YandexSDK', 'OnAdvRewarded', -1);
          }
      }
    });
  },

});