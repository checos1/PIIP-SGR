(function () {
    'use strict';
    informacionBasicaCtusIntegradoSgrController.$inject = [
        '$sce',
        '$rootScope',
        'utilidades',
        '$sessionStorage',
        'viabilidadSgrServicio',
        'justificacionCambiosServicio',
        'constantesTipoConceptoViabilidad'
    ];

    function informacionBasicaCtusIntegradoSgrController(
        $sce,
        $rootScope,
        utilidades,
        $sessionStorage,
        viabilidadSgrServicio,
        justificacionCambiosServicio,
        constantesTipoConceptoViabilidad
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctusintegradoelaboracioninfogeneralinformacionbasica';
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.ConvertirNumero = ConvertirNumero;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true
        vm.lista;

        //Listas desplegables
        vm.listaRegionesSgr;
        vm.listaCategoriasSgr;
        vm.listaSectoresApoyo;
        vm.listaCumple = [
            { Id: 'Cumple', Nombre: 'Cumple' },
            { Id: 'No cumple', Nombre: 'No cumple' },
            { Id: 'No aplica', Nombre: 'No aplica' }
        ];
        vm.Comunidad = [
            { Id: 11, Nombre: 'Comunidad indígena' },
            { Id: 15, Nombre: 'Comunidad NARP' },
            { Id: 16, Nombre: 'Comunidad Room' }
        ];

        vm.data;
        vm.form = {
            proyectoId: vm.proyectoId,
            instanciaId: vm.idInstancia,
            categoriasProyecto: [],
            regionSgr: '',
            sectorApoyo1: '',
            sectorApoyo2: '',
            valorInterventoria: '',
            valorApoyoSupervision: '',
            alcanceEspacial: '',
            poblacion: '',
            necesidadesSocioCulturales: '',
            normasLineamientosAdicionales: '',
            tipoConceptoViabilidadId: '',
            ProyectoPresentadoComunidad: 0,
            ProyectoLocalizadoComunidad: 0,
            InstanciaAprobacion: ''
        }

        vm.categorias = [];

        vm.init = function () {
            var tipoConceptoViabilidad = constantesTipoConceptoViabilidad.Integrado;
            LeerInformacionGeneral(vm.proyectoId, vm.idInstancia, tipoConceptoViabilidad);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });            
        };

        function LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode) {
            viabilidadSgrServicio.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode)
                .then(function (response) {
                    if (response.data != null) {
                        vm.data = response.data;
                        vm.data.ValorInterventoria = vm.ConvertirNumero(vm.data.ValorInterventoria);
                        vm.data.ValorApoyoSupervision = vm.ConvertirNumero(vm.data.ValorApoyoSupervision);
                        vm.data.originalInstanciaprobacion = vm.data.InstanciaAprobacion;
                        vm.data.InstanciaAprobacion = $sce.trustAsHtml(vm.data.InstanciaAprobacion.replace(/\r?\n/g, '<br>'));
                        if (response.data.Categorias != null && response.data.Categorias != "") {
                            vm.categorias = response.data.Categorias.split(",");
                        }
                        var activarInvolucrados = response.data.ProyectoViabilidadId == null ? false : true;
                        $rootScope.$broadcast("GuardadoInformacionBasica", activarInvolucrados);
                    }
                })
                .then(function () {
                    ObtenerParametricas(vm.proyectoId, vm.IdNivel);
                })
                .catch(function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer la información del proyecto.');
                    return "";
                });
        }

        function ObtenerParametricas(proyectoId, nivelId) {
            viabilidadSgrServicio.SGR_Viabilidad_LeerParametricas(proyectoId, nivelId)
                .then(function (response) {
                    if (response.data != null && response.data.length > 0) {
                        vm.listaRegionesSgr = response.data[0].RegionesSgr;
                        vm.listaCategoriasSgr = response.data[0].CategoriasSgr;
                        vm.listaSectoresApoyo = response.data[0].Sectores;

                        vm.listaCategoriasSgr.forEach(lc => {
                            if (vm.categorias.find(x => x == lc.Id)) {
                                lc.checked = true;
                            }
                        });
                    }
                }, function (error) {
                    //utilidades.mensajeError('Ocurrió un problema al leer las paramétricas.');
                    return "";
                });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarG").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarG").html("EDITAR");
                    vm.activar = true;
                    vm.init();

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        // Función para convertir un valor en formato de texto a un número flotante
        function parseValor(valor) {
            // Verifica si el valor es una cadena de texto y no es undefined
            if (typeof valor === 'string') {
                // Reemplaza los puntos por nada y las comas por puntos, luego convierte a número flotante
                return parseFloat(valor.replaceAll(".", "").replace(",", "."));
            }
            // Si el valor no es una cadena, retorna NaN o un valor predeterminado
            return NaN;
        }

        vm.guardar = function (response) {
            vm.form.categoriasProyecto = vm.listaCategoriasSgr.filter(x => x.checked).map(x => x.Id).join(",");
            vm.form.tipoConceptoViabilidadId = vm.data.TipoConceptoViabilidadId;
            vm.form.regionSgr = vm.data.RegionSgrId;
            vm.form.sectorApoyo1 = vm.data.SectorApoyo1Id;
            vm.form.sectorApoyo2 = vm.data.SectorApoyo2Id;
            //vm.form.valorInterventoria = parseFloat(vm.data.ValorInterventoria.replaceAll(".", "").replace(",", "."));
            //vm.form.valorApoyoSupervision = parseFloat(vm.data.ValorApoyoSupervision.replaceAll(".", "").replace(",", "."));
            // Asigna los valores a las propiedades del formulario, usando la función parseValor
            vm.form.valorInterventoria = parseValor(vm.data.ValorInterventoria);
            vm.form.valorApoyoSupervision = parseValor(vm.data.ValorApoyoSupervision);
            vm.form.InstanciaAprobacion = vm.data.originalInstanciaprobacion;
            vm.form.alcanceEspacial = vm.data.AlcanceEspacial;
            vm.form.poblacion = vm.data.Poblacion;
            vm.form.necesidadesSocioCulturales = vm.data.NecesidadesSocioCulturales;
            vm.form.normasLineamientosAdicionales = vm.data.NormasLineamientosAdicionales;
            vm.form.ProyectoLocalizadoComunidad = vm.data.ProyectoLocalizadoComunidad;
            vm.form.ProyectoPresentadoComunidad = vm.data.ProyectoPresentadoComunidad;

            //if (vm.form.categoriasProyecto === null || vm.form.categoriasProyecto === undefined || vm.form.categoriasProyecto === "") {
            //    utilidades.mensajeError('El campo Categoría es obligatorio', false);
            //    return;
            //}
            //if (vm.form.regionSgr === "" || vm.form.regionSgr === null || vm.form.regionSgr === undefined) {
            //    utilidades.mensajeError('El campo Región SGR es obligatorio', false);
            //    return;
            //}
            //if (vm.form.alcanceEspacial == "" || vm.form.alcanceEspacial == null || vm.form.alcanceEspacial == undefined) {
            //    utilidades.mensajeError('El campo Alcance espacial es obligatorio', false);
            //    return;
            //}
            //if (vm.form.poblacion == "" || vm.form.poblacion == null || vm.form.poblacion == undefined) {
            //    utilidades.mensajeError('El campo Población es obligatorio', false);
            //    return;
            //}
            //if (vm.form.necesidadesSocioCulturales == "" || vm.form.necesidadesSocioCulturales == null || vm.form.necesidadesSocioCulturales == undefined) {
            //    utilidades.mensajeError('El campo Responde a las necesidades socioculturales, económicas, o ambientales es obligatorio', false);
            //    return;
            //}

            Guardar();
        }

        function Guardar() {
            return viabilidadSgrServicio.SGR_Viabilidad_GuardarInformacionBasica(vm.form).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        if (response.data.Exito) {
                            $rootScope.$broadcast("GuardadoInformacionBasica", true);
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("Operación realizada con éxito!!", false, false, false);

                            $("#EditarG").html("EDITAR");
                            vm.activar = true;
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.validarTamanio = function (event) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        }

        vm.validarTamanioTexto = function (event, tamanioPermitido) {

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanio = event.target.value.length;

            if (tamanio > tamanioPermitido) {
                event.target.value = event.target.value.toString().substring(0, tamanioPermitido)
            }
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 16) {
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero, decimals = true) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: decimals ? 2 : 0,
            }).format(numero);
        }

        vm.CheckLocalizacionPresentacion = function (idComunidad, ComunidadIndigena, ComunidadNarp, ComunidadRrom) {
            return (idComunidad === 11 && ComunidadIndigena) ||
                (idComunidad === 15 && ComunidadNarp) ||
                (idComunidad === 16 && ComunidadRrom);
        };

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'SGRERRSEC') {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            //var campomensajeerror = document.getElementById(tipoError + seccion);

            var selector = `[id^="campomediocomplicado"]`;
            var elementos = document.querySelectorAll(selector);
            elementos.forEach(function (elemento) {
                if (elemento != undefined) {
                    if (!esValido) {
                        elemento.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                        elemento.classList.remove('hidden');
                    }
                    else {
                        elemento.classList.remove("ico-advertencia");
                    }
                }
                
            });
            
            //if (campomensajeerror != undefined) {
            //    if (!esValido) {
            //        campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
            //        campomensajeerror.classList.remove('hidden');
            //    } else {
            //        campomensajeerror.classList.remove("ico-advertencia");
            //    }
            //}
        }

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }

        vm.actualizainput = function (event) {
            $(event.target).val(function (index, value) {

                if (Number.isNaN(value)) {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                if (value == null) {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                if (value == "") {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                value = parseFloat(value.replaceAll(".", "").replace(",", "."));

                const val = value;
                const decimalCnt = val.toString().split('.')[1] ? val.toString().split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        vm.actualizainput2 = function (event) {
            $(event.target).val(function (index, value) {

                if (Number.isNaN(value)) {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                if (value == null) {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                if (value == "") {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                value = parseFloat(value.replaceAll(".", "").replace(",", "."));

                const val = value;
                const decimalCnt = val.toString().split('.')[1] ? val.toString().split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }
    }


    angular.module('backbone').component('informacionBasicaCtusIntegradoSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/informacionBasica/informacionBasicaCtusIntegradoSgr.html",
        controller: informacionBasicaCtusIntegradoSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        });;
})();