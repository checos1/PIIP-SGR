
angular.module('backbone.core.directives')
    .directive('maskRegex', function () {
        return {
            restrict: 'A',
            scope: {
                type: '@maskType',
                options: '=?maskOptions'
            },
            controller: ['$scope', function ($scope) { 
                
                //#region variables
                
                const _acceptedChars = {
                    allChars: /()/gi,
                    alphabetic: /[^a-zA-Z ]/gi,
                    alphabeticWithoutSpace: /[^a-zA-Z]/gi,
                    specialChars: /[\w\s]/gi,
                    alphanumeric: /[^a-zA-Z0-9 ]/g,
                    alphanumericWithoutSpace: /[^a-zA-Z0-9]/g,
                    currency: /[^0-9\.]|((?<=\d{1})\.{1,}(?!\d{1,2}))/gi,
                    numeric: /[^0-9]/gi,
                    datetime: /[^0-9]|((?<=.{10,})\d{1,})/gi
                };

                const _masksDefault = {
                    "mask-currency": {
                        pattern: /((?<!\.)\d{1}(?=(\d{3})+(?!\d)))/gi,
                        acceptedChars: 'currency',
                        attrs: {
                            placeholder: '$',
                        },
                        outputFormat: '$1,'
                    },
                    "mask-numeric": {
                        pattern: /[^0-9]/gi,
                        acceptedChars: 'numeric',
                        attrs: {
                            placeholder: "Valor",
                        }
                    },
                    "mask-datetime": {
                        pattern: /((?<=\d{0})\d{4}(?=\d{1,2}))((?<=\d{4})\d{0,2}(?=\d{1,2}))((?<=\d{4})\d{0,2})/gi,
                        acceptedChars: 'datetime',
                        attrs: {
                            placeholder: '____/__/__',
                        },
                        outputFormat: "$1/$2/$3"
                    },
                    "mask-none": {
                        pattern: /()/gi,
                        acceptedChars: "allChars",
                        attrs: { },
                        outputFormat: "",
                    }
                }

                const keys = { 
                    "arrowleft": 37, 
                    "arrowup": 38, 
                    "arrowright": 39, 
                    "arrowdown": 40, 
                    "enter": 13, 
                    "escape": 27, 
                    "tab": 9,
                    "alt": 18,
                    "backspace": 8,
                    " ": 32
                };

                $scope.masksDefault = _masksDefault;
                $scope.acceptedChars = _acceptedChars;
                $scope.keys = keys;

                //#endregion

                //#region Metodos

                function handleInput(input, options) {
                    const regexAccepted = options.acceptedCharsRegex || _acceptedChars[options.acceptedChars];
                    let value = input.value;

                    if(options.acceptedChars)
                        value = fnApplyRegex(value, regexAccepted);

                    const regex = options.pattern;
                    const outputFormat = options.outputFormat;
                    const output = fnApplyRegex(value, regex, outputFormat);
                    input.value = output;
                }

                function checkFailRegex(value, regex) {
                    return regex.test(value);
                }
        
                function fnApplyRegex(value, regex, outputFormat) {
                    return value.replace(regex, outputFormat || '')
                }

                function fnApplyAttrs(input, $scope){
                    for (const key in ($scope.options.attrs || $scope.attrsParent || []))
                    if(input.attributes[key])
                        input.attributes[key].value = ($scope.options && $scope.options.attrs[key]) 
                            || ($scope.attrsParent && $scope.attrsParent[key].value)
                            || input.attributes[key].value;
                }

                function deepEqual (objeto, compare) {
                    if(!objeto || !compare)
                        return false;

                    const keys1 = Object.keys(objeto);
                    const keys2 = Object.keys(compare);
                  
                    if (keys1.length !== keys2.length)
                      return false;
                  
                    for (const key of keys1) {
                      const val1 = objeto[key];
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
            
                $scope.handleInput = handleInput;
                $scope.fnApplyRegex = fnApplyRegex;
                $scope.checkFailRegex = checkFailRegex;
                $scope.fnApplyAttrs = fnApplyAttrs;
                $scope.deepEqual = deepEqual;

                //#endregion
            }],
            link: function ($scope, element) {
                const input = element[0];
                $scope.attrsParent = Object.create(input.attributes);

                setTimeout(() => $scope.$apply(), 250);

                $scope.$watch('type', function (newType, oldType) {
                    if(!newType){
                        removeEvents(input);
                        return
                    };

                    if (oldType == newType ) return;

                    registerEvents(input);
                    $scope.options = $scope.masksDefault[newType];
                    $scope.fnApplyAttrs(element[0], $scope);
                });

                $scope.$watch('options', function (newOptions, oldOptions) {
                    if(!newOptions) {
                        removeEvents(input);
                        return;
                    }

                    if ($scope.deepEqual(oldOptions, newOptions)) return;

                    registerEvents(input);
                    $scope.options = newOptions;
                    $scope.fnApplyAttrs(element[0], $scope);
                });

                if(!$scope.type)
                    return;

                let options = $scope.options = $scope.options || $scope.masksDefault[$scope.type || "mask-none"];
                $scope.fnApplyAttrs(input, $scope);
                registerEvents(input);

                function registerEvents(input){
                    input.addEventListener("keydown", keyDownEvent);
                    input.addEventListener("keyup", keyUpEvent);
                    input.addEventListener("blur", blurEvent);
                }

                function removeEvents(input){
                    input.removeEventListener("keydown", keyDownEvent);
                    input.removeEventListener("keyup", keyUpEvent);
                    input.removeEventListener("blur", blurEvent);
                }

                function keyUpEvent(event) {
                    const keyPressed = $scope.keys[(event.key || "").toLowerCase()]
                    if(keyPressed && keyPressed != $scope.keys.backspace)
                        return;

                    $scope.handleInput(input, $scope.options);
                }

                function keyDownEvent (event) {
                    if($scope.keys[(event.key || "").toLowerCase()])
                        return;

                    const valor = `${input.value}${event.key}`;
                    const acceptedCharsRegex = $scope.options.acceptedCharsRegex || $scope.acceptedChars[$scope.options.acceptedChars];
                    if($scope.checkFailRegex(valor, acceptedCharsRegex))
                        return;
                }

                function blurEvent () {
                    $scope.handleInput(input, $scope.options)
                }
            }
        };
    });