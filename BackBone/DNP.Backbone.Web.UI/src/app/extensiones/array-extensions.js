(function() {
    angular.module('backbone.extensions').factory('array.extensions', [function() {
        const isRequired = (value) => new Error(`parámetros ${value} es obligatório`);

        Array.prototype.removeAt = function (index = isRequired('index')) {
            this.splice(index, 1);
            return this;
        }
        
        Array.prototype.remove = function (item = isRequired('item')) {
            const index = this.indexOf(item);
            return this.removeAt(index);
        }

        Array.prototype.distinct = function() {
            return this.filter((x, index, arr) => arr.indexOf(x) === index);
        }

        Array.prototype.distinctBy = function(expr, fnFormat = null) {
            const distinct = Array.from(new Set(this.map(expr)))
            if(fnFormat)
                return distinct.map(fnFormat);

            return distinct;
        }
    
        return Array.prototype;
    }])
})();