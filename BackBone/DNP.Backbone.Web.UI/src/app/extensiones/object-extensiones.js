(function() {
    angular.module('backbone.extensions').factory('object.extensions', [function() {
        
      const deepEqual = function (compare) {
        const keys1 = Object.keys(this);
        const keys2 = Object.keys(compare);
      
        if (keys1.length !== keys2.length)
          return false;
      
        for (const key of keys1) {
          const val1 = this[key];
          const val2 = compare[key];
          const areObjects = isObject(val1) && isObject(val2);
          if (areObjects && !deepEqual(val1, val2) || !areObjects && val1 !== val2)
            return false;
        }
      
        return true;
      }
    
      function isObject(object) {
          return object != null && typeof object === 'object';
      }

      Object.prototype.deepEqual = deepEqual;
      return Object.prototype;
    }])
})();