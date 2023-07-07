mergeInto(LibraryManager.library, {

  RefreshDatabase: function () {
    FS.syncfs(false, function (err) {
      console.log('Failed to refresh IndexedDB with error:', err);
    });
  },

});