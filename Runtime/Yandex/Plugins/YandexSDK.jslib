mergeInto(LibraryManager.library, {

  SaveDataExtern: function (data) {
    var dataString = UTF8ToString(data);
    var dataObj = JSON.parse(dataString);
    player.setData(dataObj).then(() => {
      gameInstance.SendMessage('YandexSDK', 'OnDataSaved');
    });
  },

  LoadDataExtern: function () {
    player.getData().then(data => {
      var dataString = JSON.stringify(data);
      gameInstance.SendMessage('YandexSDK', 'OnDataLoaded', dataString);
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
            gameInstance.SendMessage('YandexSDK', 'OnAdvRewarded');
          },
          onClose: () => {
            console.log('Video adv closed.');
          },
          onError: (error) => {
            console.log('Video adv opened with error:', error);
          }
      }
    });
  },

});