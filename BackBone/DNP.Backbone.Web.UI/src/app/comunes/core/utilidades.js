(function () {

    'use strict';
    angular.module('backbone')
        .factory('utilidades', utilidades);

    utilidades.$inject = ['$location', '$timeout', '$localStorage', '$q', '$sessionStorage'];

    function utilidades($location, $timeout, $localStorage, $q, $sessionStorage) {

        var servicios = {
            isNull: function (value) {
                if (value === undefined || value === null || value === '') {
                    return true;
                }
                return false;
            },
            isNotNull: function (value) {
                if (value != undefined && value != null && value != '') {
                    return true;
                }
                return false;
            },
            isTrue: function (value) {
                if (value === true) {
                    return true;
                }
                return false;
            },
            arrayHasRows: function (value) {
                if (value.length > 0) {
                    return true;
                }
                return false;
            },
            replaceAll: function (text, search, replaceWith) {
                return text.replace(new RegExp(search, 'g'), replaceWith);
            },
            httpRequestComplete: function (results) {
                return results.data;
            },
            httpRequestError: function (results) {
                if (results.status === 401) {
                    $location.path('/');
                }

                if (results.data) {
                    return results.data.ExceptionMessage;
                }

                return undefined;
            },
            httpRequestErrorReject: function (results) {
                if (results.status === 401) {
                    $location.path('/');
                }

                var excepcion = results.data ? results.data.ExceptionMessage : undefined;

                return $q.reject(excepcion);
            },
            generatePasswordRand: function (length, type) {
                let characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                switch(type){
                case 'num':
                            characters = "0123456789";
                            break;
                case 'alf':
                            characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            break;
                case 'rand':
                            //FOR ↓
                            characters = "01234abcdefghijklmNOPQRSTUVWXYZ56789ABCDEFGHIJKLMnopqrstuvwxyz!#$%&/()=?¡*+-_.,;{}[]";
                            break;
                default:
                            characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            break;
                        }
                let pass = "";
                for (i = 0; i < length; i++) {
                    //if (type == 'rand') {
                    //    pass += String.fromCharCode((Math.floor((Math.random() * 100)) % 94) + 33);
                    //} else {
                        pass += characters.charAt(Math.floor(Math.random() * characters.length));
                    //}
                }
                return pass;
            },
            tienePermiso: function (permisosOpciones, idPantalla, vm) {
                var resultado = false;
                _.filter(permisosOpciones,
                    function (elemento) {
                        if (elemento.IdOpcionDnp === idPantalla) {
                            resultado = true;
                            vm.nombreOpcion = elemento.NombreOpcion;
                        }
                    });
                return resultado;
            },
            mensajeError: function (mensaje, funcionContinuar, titulo, confirmButtonText) {
                swal({
                    title: `${(titulo == null) ? 'Error' : titulo}`,
                    text: `${mensaje} ${usuarioDNP == '' ? '' : "<button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>"}`,
                    type: "error",
                    confirmButtonText: `${(confirmButtonText == '' || confirmButtonText == null || confirmButtonText == undefined) ? 'Aceptar' : confirmButtonText}`,
                    closeOnConfirm: true,
                    customClass: 'sweet-alertTittleError',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionContinuar === "function") {
                            funcionContinuar();
                        }
                    });
            },
            mensajeWarning: function (mensaje, funcionContinuar, funcionCancelar, confirmButtonText, cancelButtonText, titulo) {
                confirmButtonText = confirmButtonText || "Aceptar";
                cancelButtonText = cancelButtonText || "Cancelar";
                swal({
                    title: `${(titulo == null) ? 'Advertencia' : titulo}`,
                    text: "<div style='padding-bottom:2rem !important;'>" + mensaje + "</div> <button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: confirmButtonText,
                    cancelButtonText: cancelButtonText,
                    closeOnConfirm: true,
                    customClass: 'sweet-alertTittleWarnning',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionContinuar === "function") {
                            funcionContinuar();
                        } else if (typeof funcionCancelar === "function") {
                            funcionCancelar();
                        }
                    });
            },
            mensajeSuccess: function (mensaje, showCancelButton, funcionContinuar, funcionCancelar, titulo, confirmButtonText, cancelButtonText) {
                confirmButtonText = confirmButtonText || "Aceptar";
                cancelButtonText = cancelButtonText || "Cancelar";
                showCancelButton = showCancelButton || false;
                swal({
                    title: `${(titulo == null) ? 'Los datos fueron guardados con éxito' : titulo}`,
                    text: "<div style='padding-bottom:2rem !important;'>" + mensaje + "</div><button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>",
                    type: "success",
                    showCancelButton: showCancelButton,
                    confirmButtonText: confirmButtonText,
                    cancelButtonText: cancelButtonText,
                    closeOnConfirm: true,
                    closeOnCancel: true,
                    customClass: 'sweet-alertTittleSucces',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionContinuar === "function") {
                            funcionContinuar();
                        } else if (typeof funcionCancelar === "function") {
                            funcionCancelar();
                        }
                        else if (typeof funcionContinuar === "function" && typeof funcionContinuar != null && typeof funcionContinuar != false) {
                            funcionContinuar();
                        }
                    });
            },
            mensajeSuccessRedirect: function (mensaje, funcionConfirmar1, funcionConfirmar2, titulo, confirmButtonText1, confirmButtonText2) {        
                swal({
                    title: `${(titulo == null) ? 'Los datos fueron guardados con éxito' : titulo}`,
                    text: "<div style='padding-bottom:2rem !important;'>" + mensaje,
                    type: "success",
                    showCancelButton: true,
                    confirmButtonText: confirmButtonText1,
                    cancelButtonText: confirmButtonText2,
                    reverseButtons: true,
                    customClass: 'sweet-alertTittleSucces',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionConfirmar1 === "function") {
                            funcionConfirmar1();
                        } else if (typeof funcionConfirmar2 === "function") {
                            funcionConfirmar2();
                        }
                    });

                var cancelButton = document.querySelector('.sweet-alert .sa-button-container .cancel');
                cancelButton.style.cssText = 'background-color: rgb(51, 102, 204) !important; font-size: 14px !important; color: white !important; border-color: #ffffff !important'; // Cambiar al color deseado        
                cancelButton.addEventListener('mouseover', function () {
                    cancelButton.style.cssText = 'background-color: rgb(49, 98, 196) !important; font-size: 14px !important; color: white !important; border-color: #ffffff !important'; // Cambiar el color al pasar el mouse
                });  
                cancelButton.addEventListener('mouseout', function () {
                    cancelButton.style.cssText = 'background-color: rgb(51, 102, 204) !important; font-size: 14px !important; color: white !important; border-color: #ffffff !important'; // Restaurar el color original
                });
            },
            mensajeInformacionN: function (mensaje, funcionContinuar, titulo, texto = '') {
                if (titulo == null) {
                    titulo = "Información";
                }

                if (texto == '') {
                    texto = "Esta es la explicación de  etc etc....un hecho establecido hace demasiado tiempo que un lector se distraerá con el contenido del texto de un sitio mientras que mira su diseño. El punto de usar Lorem Ipsum es que tiene una distribución más o menos normal de las letras, al contrario de usar textos como por ejemplo Contenido aquí, contenido aquí.";
                }

                swal({
                    title: "<button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>" + titulo,
                    text: texto,
                    type: "info",
                    confirmButtonText: "Aceptar",
                    closeOnConfirm: true,
                    customClass: 'sweet-alertTittleInfo2',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionContinuar === "function") {
                            funcionContinuar();
                        }
                    });

                document.getElementById("pMensaje").innerHTML = mensaje;
            },
            ConverNum2Decimal: function (numero) {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2,
                }).format(numero);
            },
            ConverNum4Decimal: function (numero) {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 4,
                    maximumFractionDigits: 4,
                }).format(numero);
            },
            mensajeInformacionV: function (mensaje, funcionContinuar, titulo) {
                if (titulo == null) {
                    titulo = "Información";
                }


                swal({
                    title: "<button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button><div style='font-size:16px !important;font-weight:700 !important;margin-top: 25px !important;padding-bottom:0.5rem !important;'>¿Que es esto?</div><div style='font-size:16px !important;font-weight:500;' >" + titulo + "</div>",
                    text: "<div style='padding-bottom:2rem !important;'>" + mensaje + "</div>",
                    type: "info",
                    confirmButtonText: "Aceptar",
                    closeOnConfirm: true,
                    customClass: 'sweet-alertTittleInfo2',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionContinuar === "function") {
                            funcionContinuar();
                        }
                    });

                document.getElementById("pMensaje").innerHTML = mensaje;
            },
            mensajeInformacion: function (mensaje, funcionContinuar, titulo) {
                swal({
                    title: `<p style='font-size:16px !important;font-weight:700 !important;margin-top: 25px !important;'>¿Que es esto?</p><p style='font-size:18px;padding-bottom:20px !important;font-family:'Montserrat Medium', 'Montserrat Regular', 'Montserrat';font-weight:500;>${(titulo == null) ? 'Información' : titulo}</p>`,
                    text: `<p style='text-align:left !important; margin-top:25px; overflow:scroll;' id='pMensaje'>${mensaje}</p>     <button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;padding-top:30px !important;'>×</button>`,
                    confirmButtonText: "Aceptar",
                    closeOnConfirm: true,
                    customClass: 'sweet-alert-info',
                    html: true
                },
                    function (isConfirm) {
                        if (isConfirm && typeof funcionContinuar === "function") {
                            funcionContinuar();
                        }
                    });

            },
            // ReSharper disable once UnusedParameter
            cambiosSinGuardar: function (hayCambios, mensaje, $sessionStorage, event, next, current) {
                $sessionStorage.idPantalla = '';

                if (hayCambios) {

                    event.preventDefault();

                    swal({
                        title: "",
                        text: mensaje,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Continuar",
                        cancelButtonText: "Cancelar",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                if ((next.indexOf("/Account/SignOut") > -1)) {
                                    delete $localStorage.authorizationData;
                                }
                                window.location.href = next;
                            }

                        });
                } else {

                    if ((next.indexOf("/Home/LogOff") === -1)) {
                        event.preventDefault();
                        window.location.href = next;
                    }
                }
            },
            descargarBlob: function (archivoBlog, nombreArchivo) {
                if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                    window.navigator.msSaveOrOpenBlob(archivoBlog, nombreArchivo);
                } else {
                    var tempLink = document.createElementNS('http://www.w3.org/1999/xhtml', 'a'); //document.createElement('a');
                    var blobUrl = window.URL.createObjectURL(archivoBlog);
                    tempLink.setAttribute("href", blobUrl);
                    tempLink.setAttribute('download', nombreArchivo);
                    document.body.appendChild(tempLink);
                    tempLink.click();
                    document.body.removeChild(tempLink);
                }
            },
            estiloRegistroActivo: function (estadoExpire) {
                if (estadoExpire) {
                    return 'expire';
                }
                return 'color: black;';
            },
            obtenerFechaDia: function () {
                var d = new Date();
                d.setHours(0, 0, 0, 0);
                return d;
            },
            obtenerBlobArchivoBase64: function (archivoBase64) {
                var sliceSize = 512;

                var byteCharacters = Base64.decode(archivoBase64);
                var byteArrays = [];

                for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                    var slice = byteCharacters.slice(offset, offset + sliceSize);

                    var byteNumbers = new Array(slice.length);
                    for (var i = 0; i < slice.length; i++) {
                        byteNumbers[i] = slice.charCodeAt(i);
                    }

                    var byteArray = new Uint8Array(byteNumbers);
                    byteArrays.push(byteArray);
                }

                return new Blob(byteArrays);
            },
            refrescarScope: function ($scope, tiempoMilisegundos) {
                var tiempo = !tiempoMilisegundos ? 500 : tiempoMilisegundos;
                $timeout(function () {
                    $scope.$apply();
                }, tiempo);
            },
            validarEmail: function (valor) {
                if (/^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i.test(valor)) {
                    return true;
                }
                return false;
            },
            obtenerExtensionArchivo: function (fileName) {
                var arrSplit = fileName.split('.');
                return arrSplit[arrSplit.length - 1];
            },
            generarGuid: function () {
                function generarCuarteto() {
                    return Math.floor((1 + Math.random()) * 0x10000)
                        .toString(16)
                        .substring(1);
                }

                return generarCuarteto() + generarCuarteto() + '-' + generarCuarteto() + '-' + generarCuarteto() + '-' + generarCuarteto() + '-' + generarCuarteto() + generarCuarteto() + generarCuarteto();
            },
            generarLogExcepcionHTML: function (excepcion) {
                var titulo = '<div class="container">' + excepcion.Mensaje;
                var inicioTabla = "<div ><table class='table table-bordered table-error'>";
                var cabeceraDeTabla = '<thead><tr><th class="text-center">Error</th></tr></thead>';
                var cuerpoDeLaTabla = '<tbody>';
                var finDeLaTabla = '</tbody></table></div></div>';

                _.each(excepcion.ListaErrores, function (error) {
                    cuerpoDeLaTabla += '<tr><td>' + error.Error + '</td><tr/>';
                });

                return titulo + inicioTabla + cabeceraDeTabla + cuerpoDeLaTabla + finDeLaTabla;
            },
            toNormalForm: function (str) {
                return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
            },
            base64toBlob: function (base64Data, contentType) {
                contentType = contentType || '';
                var sliceSize = 1024;
                var byteCharacters = atob(base64Data);
                var bytesLength = byteCharacters.length;
                var slicesCount = Math.ceil(bytesLength / sliceSize);
                var byteArrays = new Array(slicesCount);

                for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
                    var begin = sliceIndex * sliceSize;
                    var end = Math.min(begin + sliceSize, bytesLength);

                    var bytes = new Array(end - begin);
                    for (var offset = begin, i = 0; offset < end; ++i, ++offset) {
                        bytes[i] = byteCharacters[offset].charCodeAt(0);
                    }
                    byteArrays[sliceIndex] = new Uint8Array(bytes);
                }
                return new Blob(byteArrays, { type: contentType });
            },
            obtenerParametroTransversal(str) {
                var valor;
                if (typeof $sessionStorage.parametros !== undefined) { 
                    valor = $sessionStorage.parametros.find(({ Parametro }) => Parametro === str).Valor;
                }
                return valor;
            }
        }

        return servicios;
    }

})();