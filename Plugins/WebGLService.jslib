mergeInto(LibraryManager.library, {

  UpdateIndexedDB: function () {
    FS.syncfs(false, function (err) {
      console.log('IndexedDB updated.');
      if (err) {
        console.log('Failed to update IndexedDB with error:', err);
      }
    });
  },
});