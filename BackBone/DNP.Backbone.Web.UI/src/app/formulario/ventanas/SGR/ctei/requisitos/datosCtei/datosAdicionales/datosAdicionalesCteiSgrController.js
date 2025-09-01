(function () {
    'use strict';
    datosAdicionalesCteiSgrController.$inject = [    
        '$scope',
        'utilidades',
        '$sessionStorage',
        'requisitosViabilidadCteiSgrServicio',
        'justificacionCambiosServicio',
    ];

    function datosAdicionalesCteiSgrController(    
        $scope,
        utilidades,
        $sessionStorage,
        requisitosViabilidadCteiSgrServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrcteidatoscteidatosadicionalesctei";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        vm.ConvertirNumero = ConvertirNumero;
        vm.nombreLista;

        vm.codigoG;
        vm.exitoFind = false;
        vm.wFiltro = false;
        vm.preguntaCredito = 3816;
        $sessionStorage.habilitaOperacionCredito = false;

        vm.disabled = $sessionStorage.soloLectura;
        vm.activar = false;

        vm.datosAdicionalesCTEI = {
            Id: 0,
            ProyectoId: 0,
            InstanciaId: "",
            ProgramaEstrategia: 0,
            PAED: "",
            AtencionDeDesastres: 0,
            EnfoqueDiferencial: 0,
            Minorias: 0,
            ConcordanteConAcuerdos: 0,
            SolicitaVigenciasFuturas: 0,
            RequiereCertificado: 0,
            ValorInterventoria: 0.00,
            ValorSupervision: 0.00
        };

        vm.revisiones = 0;
        vm.valorTotal = vm.datosAdicionalesCTEI.ValorInterventoria + vm.datosAdicionalesCTEI.ValorSupervision;

        vm.textoAtencionDeDesastres = "";
        vm.textoEnfoqueDiferencial = "";
        vm.textoMinorias = "";
        vm.textoConcordanteConAcuerdos = "";
        vm.textoSolicitaVigenciasFuturas = "";
        vm.textoRequiereCertificado = "";

        vm.programasEstrategiasCTEI = [];

        vm.CalcularTotalFuentes = CalcularTotalFuentes;

        vm.init = function () {
            ObtenerDatosAdicionalesCTEI();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        function ObtenerDatosAdicionalesCTEI() {
            requisitosViabilidadCteiSgrServicio.SGR_Proyectos_LeerDatosAdicionalesCTEI(vm.proyectoId, vm.idInstancia).then(
                function (result) {
                    var respuestaJson = JSON.parse(result.data);
                    vm.datosAdicionalesCTEI = respuestaJson.DatosAdicionalesCTEI;
                    if (Object.keys(vm.datosAdicionalesCTEI).length > 0) { 
                        if (typeof vm.datosAdicionalesCTEI.AtencionDeDesastres !== "undefined") { vm.datosAdicionalesCTEI.AtencionDeDesastres = vm.datosAdicionalesCTEI.AtencionDeDesastres.toString(); }
                        if (typeof vm.datosAdicionalesCTEI.EnfoqueDiferencial !== "undefined") { vm.datosAdicionalesCTEI.EnfoqueDiferencial = vm.datosAdicionalesCTEI.EnfoqueDiferencial.toString(); }
                        if (typeof vm.datosAdicionalesCTEI.Minorias !== "undefined") { vm.datosAdicionalesCTEI.Minorias = vm.datosAdicionalesCTEI.Minorias.toString(); }
                        if (typeof vm.datosAdicionalesCTEI.ConcordanteConAcuerdos !== "undefined") { vm.datosAdicionalesCTEI.ConcordanteConAcuerdos = vm.datosAdicionalesCTEI.ConcordanteConAcuerdos.toString(); }
                        if (typeof vm.datosAdicionalesCTEI.SolicitaVigenciasFuturas !== "undefined") { vm.datosAdicionalesCTEI.SolicitaVigenciasFuturas = vm.datosAdicionalesCTEI.SolicitaVigenciasFuturas.toString(); }
                        if (typeof vm.datosAdicionalesCTEI.RequiereCertificado !== "undefined") { vm.datosAdicionalesCTEI.RequiereCertificado = vm.datosAdicionalesCTEI.RequiereCertificado.toString(); }
                    }
                    vm.datosAdicionalesCTEI.ProyectoId = vm.proyectoId;
                    vm.datosAdicionalesCTEI.InstanciaId = vm.idInstancia;
                    vm.revisiones = respuestaJson.Revisiones;
                    vm.textoAtencionDeDesastres = respuestaJson.TextoAtencionDeDesastres;
                    vm.textoEnfoqueDiferencial = respuestaJson.TextoEnfoqueDiferencial;
                    vm.textoMinorias = respuestaJson.TextoMinorias;
                    vm.textoConcordanteConAcuerdos = respuestaJson.TextoConcordanteConAcuerdos;
                    vm.textoSolicitaVigenciasFuturas = respuestaJson.TextoSolicitaVigenciasFuturas;
                    vm.textoRequiereCertificado = respuestaJson.TextoRequiereCertificado;
                    vm.programasEstrategiasCTEI = respuestaJson.ProgramasEstrategiasCTEI;

                    CalcularTotalFuentes();

                    if (typeof vm.datosAdicionalesCTEI.ValorInterventoria !== "undefined" && typeof vm.datosAdicionalesCTEI.ValorInterventoria === "number") { vm.datosAdicionalesCTEI.ValorInterventoria = vm.ConvertirNumero(vm.datosAdicionalesCTEI.ValorInterventoria); }
                    if (typeof vm.datosAdicionalesCTEI.ValorSupervision !== "undefined" && typeof vm.datosAdicionalesCTEI.ValorSupervision === "number") { vm.datosAdicionalesCTEI.ValorSupervision = vm.ConvertirNumero(vm.datosAdicionalesCTEI.ValorSupervision); }

                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }


        vm.ActivarEditar = function () {
            if (vm.activar == false) {
                $("#EditarG2").html("CANCELAR");
                vm.activar = true;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarG2").html("EDITAR");
                    vm.activar = false;
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

        vm.formatFila = function (event) {
            $(event.target).val(function (index, value) {
                return value.replaceAll(".", "");
            });
        }

        vm.actualizaFila = function (event) {

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
                const decimalCnt = val.toString().split(',')[1] ? val.toString().split(',')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }


        vm.guardar = function (response) {
            Guardar();
        }

        function Guardar() {

            if (typeof vm.datosAdicionalesCTEI.ValorInterventoria === 'string') {
                vm.datosAdicionalesCTEI.ValorInterventoria = parseFloat(vm.datosAdicionalesCTEI.ValorInterventoria.replaceAll(".","").replace(',', '.'));
            }

            if (typeof vm.datosAdicionalesCTEI.ValorSupervision === 'string') {
                vm.datosAdicionalesCTEI.ValorSupervision = parseFloat(vm.datosAdicionalesCTEI.ValorSupervision.replaceAll(".", "").replace(',', '.'));
            }

            return requisitosViabilidadCteiSgrServicio.SGR_Proyectos_GuardarDatosAdicionalesCTEI(vm.datosAdicionalesCTEI).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("", false, false, false);
                        $("#EditarG2").html("EDITAR");
                        vm.activar = false;
                        vm.init();
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        vm.guardadohijos = function (nombreComponenteHijo) {
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });


                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });

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
            permitido = event.target.value.toString().includes(",");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(",");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(",");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(",");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === ',')) return;
                    if (spiltArray.length === 2 && newValue === '-,') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + ',' + n[1].slice(0, 2);
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
            permitido = event.target.value.toString().includes(",");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(",");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(",");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === ',')) return;
                    if (spiltArray.length === 2 && newValue === '-,') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + ',' + n[1].slice(0, 2);
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

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ObtenerNumero(obj) {
            return typeof obj == 'string' ? parseFloat(obj.replace(",", ".")) : obj;
        }


        function CalcularTotalFuentes () {
            var sum = 0;
            var ValorInterventoria = 0;
            var ValorSupervision = 0;

            ValorInterventoria = typeof vm.datosAdicionalesCTEI.ValorInterventoria !== "undefined" ? typeof vm.datosAdicionalesCTEI.ValorInterventoria === "string" ? ObtenerNumero(vm.datosAdicionalesCTEI.ValorInterventoria.replaceAll(".", "")) : vm.datosAdicionalesCTEI.ValorInterventoria : 0;

            ValorSupervision = typeof vm.datosAdicionalesCTEI.ValorSupervision !== "undefined" ? typeof vm.datosAdicionalesCTEI.ValorSupervision === "string" ? ObtenerNumero(vm.datosAdicionalesCTEI.ValorSupervision.replaceAll(".", "")) : vm.datosAdicionalesCTEI.ValorSupervision : 0;

            sum = ValorInterventoria + ValorSupervision;

            vm.valorTotal = sum;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            //vm.limpiarErrores();

            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (TipoError == 'SGRVDP1') {
                            vm.validarValores2(nameArr[1].toString(), p.Descripcion, false, "A");
                        }
                        else if (TipoError == 'SGRVDP2') {
                            vm.validarValores2(nameArr[1].toString(), p.Descripcion, false, "B");
                        }
                        else if (TipoError == 'SGRERRSEC') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        }
                        else if (TipoError == 'REQ') {
                            vm.validarValoresOblig(nameArr[1].toString(), p.Descripcion, false);
                        }
                        else
                            vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }

        }

        /**
        * Función que crea las referencias de los métodos de los hijos con el padre. Este es llamado cuando se inicializa el componente hijo.
        * @param {any} handler función referenciada
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;

        };


        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionEstado = function (nombreComponente, esValido) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.showAlertError = function (nombreComponente, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    var campomensajeerror = document.getElementById(vm.nombreComponente + pregunta);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                        campomensajeerror.classList.remove('hidden');
                    }

                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValoresOblig = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alertE-" + vm.nombreComponente + '-' + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    var campomensajeerror = document.getElementById("ERR-" + vm.nombreComponente + '-' + pregunta);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.innerHTML = errores;
                        campomensajeerror.classList.remove('hidden');
                    }

                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValores2 = function (pregunta, errores, esValido, Ident) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    var campomensajeerror = document.getElementById(vm.nombreComponente + Ident + pregunta);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                        campomensajeerror.classList.remove('hidden');
                    }

                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var idSpanAlertComponent = document.getElementById("alert-sgrcteidatoscteidatosadicionalesctei-programaestrategia");
            idSpanAlertComponent.classList.remove("ico-advertencia");
        }
    }

    angular.module('backbone').component('datosAdicionalesCteiSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctei/requisitos/datosCtei/datosAdicionales/datosAdicionalesCteiSgr.html",
        controller: datosAdicionalesCteiSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificarrefresco: '&',
            notificacionestado: '&',
            guardadocomponent: '&'
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