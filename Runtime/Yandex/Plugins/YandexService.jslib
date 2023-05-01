mergeInto(LibraryManager.library, {

  GetConnectionStatus: function () {
    console.log('Connection status:', connectionStatus);
    return connectionStatus;
  },

  GetDevice: function () {
    var device = ysdk.deviceInfo.type;
    var bufferSize = lengthBytesUTF8(device) + 1;
    var buffer = _malloc(bufferSize);
    console.log('Detected device type:', device);
    stringToUTF8(device, buffer, bufferSize);
    return buffer;
  },

  GetLanguage: function () {
    var language = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(language) + 1;
    var buffer = _malloc(bufferSize);
    console.log('Detected language:', language);
    stringToUTF8(language, buffer, bufferSize);
    return buffer;
  },

  SaveData: function (data) {
    var dataString = UTF8ToString(data);
    var dataObject = JSON.parse(dataString);
    player.setData(dataObject).then(() => {
      console.log('Data saved.');
      gameInstance.SendMessage('YandexSDK', 'OnDataSaved');
    });
  },

  LoadData: function () {
    player.getData().then(data => {
      var dataString = JSON.stringify(data);
      console.log('Data loaded.');
      gameInstance.SendMessage('YandexSDK', 'OnDataLoaded', dataString);
    });
  },

  SetLeaderboard: function (id, value) {
    ysdk.isAvailableMethod('leaderboards.setLeaderboardScore').then(result => {
      if (result === true) {
        ysdk.getLeaderboards().then(lb => {
          lb.setLeaderboardScore(UTF8ToString(id), value).then(() => {
            console.log('Leaderboard updated.');
          });
        });
      };
    });
  },

  ShowFullscreenAdv: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onOpen: () => {
          console.log('Fullscreen adv opened.');
          gameInstance.SendMessage('YandexSDK', 'OnFullscreenAdvOpened');
        },
        onClose: function(wasShown) {
          console.log('Fullscreen adv closed with result:', wasShown);
          gameInstance.SendMessage('YandexSDK', 'OnFullscreenAdvClosed');
        },
        onError: function(error) {
          console.log('Fullscreen adv opened with error:', error);
        }
      }
    });
  },

  ShowRewardedAdv: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video adv opened.');
          gameInstance.SendMessage('YandexSDK', 'OnVideoAdvOpened');
        },
        onRewarded: () => {
          console.log('Video adv rewarded.');
          gameInstance.SendMessage('YandexSDK', 'OnVideoAdvRewarded', 1);
        },
        onClose: () => {
          console.log('Video adv closed.');
          gameInstance.SendMessage('YandexSDK', 'OnVideoAdvRewarded', 0);
        },
        onError: (error) => {
          console.log('Video adv opened with error:', error);
          gameInstance.SendMessage('YandexSDK', 'OnVideoAdvRewarded', -1);
        }
      }
    });
  },

});