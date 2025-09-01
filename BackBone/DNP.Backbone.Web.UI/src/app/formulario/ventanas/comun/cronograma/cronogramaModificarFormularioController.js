(function () {
	'use strict';

	cronogramaModificarFormularioController.$inject = [
		'$sessionStorage',
		'cronogramavigfuturaServicio',
		'utilidades',
		'$timeout',
		'justificacionCambiosServicio',
	];

	function cronogramaModificarFormularioController(
		$sessionStorage,
		cronogramavigfuturaServicio,
		utilidades,
		$timeout,
		justificacionCambiosServicio
	) {
		this.togglePre = "+";
		this.toggleCon = "+";
		this.editarPrecontractual = true;
		this.editarContractual = true;
		this.nombreComponente = "reprogramacionvfcronograma";
		this.idProyecto = $sessionStorage.proyectoId;
		this.isEdicion = false;
		this.idInstancia = $sessionStorage.idInstancia;
		this.instanciaSeleccionada = $sessionStorage.InstanciaSeleccionada;
		this.FechaHorizonte = $sessionStorage.InstanciaSeleccionada.Horizonte.split(' - ')[1];
		this.idFlujo = $sessionStorage.idFlujoIframe;
		this.idNivel = $sessionStorage.idNivel;
		this.TramiteId = $sessionStorage.TramiteId;
		this.ObtenerModalidadContratacion = ObtenerModalidadContratacion;
		this.cambiarModalidad = cambiarModalidad;
		this.modalidadContratacionIdAnterior = null;
		this.modalidadContratacionSeleccionada = null;
		this.cambioModalidad = false;
		this.modalidadContratacionId = null;
		this.verActividades = false;
		this.confirmarCancelarPrecontractual = false;
		this.isConstante = false;
		this.modalidadesContratacion = [];
		this.mensajeAlertaPrecontractual = "";
		this.mensajesErrorPre = [];
		this.mensajesErrorCon = [];
		this.mensajesErrorArchivo = [];

		this.actividadesPrecontractuales = [];
		this.actividadesPrecontractualesIniciales = [];
		this.actividadesContractuales = [];
		this.actividadesContractualesIniciales = [];
		this.EliminarActividadesContractuales = [];
		let actividadesIniciales = null;

		this.files = [];
		this.datosImportacion = null;
		this.esArchivoValido = false;
		this.fechaPrueba = new Date();

		this.eliminarActividades = false;

		this.togglePrecontractual = () => {
			this.togglePre = this.togglePre == '+' ? '-' : '+';
		}
		this.toggleContractual = () => {
			this.toggleCon = this.toggleCon == '+' ? '-' : '+';
		}

		this.agregarActividad = () => {
			this.HabilitaEditarContractual(false);
			this.actividadesContractuales.push({ CronogramaId: null, ModalidadContratacionId: this.modalidadContratacionId, ActividadPreContractualId: null, Actividad: '', FechaInicial: '', FechaFinal: '' });
		}

		this.HabilitaEditarPrecontractual = (valor) => {
			this.editarPrecontractual = valor;
		}

		this.HabilitaEditarContractual = (valor) => {
			this.editarContractual = valor;
		}

		this.cancelarPrecontractual = () => {
			utilidades.mensajeWarning("¿Está seguro de continuar?", () => {
				this.editarPrecontractual = true;
				for (let index = 0; index < this.actividadesPrecontractuales.length; index++) {

					this.actividadesPrecontractuales[index].FechaInicial = this.actividadesPrecontractualesIniciales[index].FechaInicial ? new Date(this.actividadesPrecontractualesIniciales[index].FechaInicial) : null;
					this.actividadesPrecontractuales[index].FechaFinal = this.actividadesPrecontractualesIniciales[index].FechaFinal ? new Date(this.actividadesPrecontractualesIniciales[index].FechaFinal) : null;
				}
				$timeout(function () {
					utilidades.mensajeSuccess("", false, () => { console.log('Restablecer actividades precontractuales'); }, null, "Se ha cancelado la edición.")

				}, 600);

			}, null, "Aceptar", "Cancelar", "Los datos que posiblemente haya diligenciado se perderán.");
		}

		this.cancelarContractual = () => {
			utilidades.mensajeWarning("¿Está seguro de continuar?", () => {
				this.editarContractual = true;
				this.actividadesContractuales = [...this.actividadesContractualesIniciales];
				for (let index = 0; index < this.actividadesContractualesIniciales.length; index++) {
					// if (this.actividadesContractualesIniciales.length != 0) {
					this.actividadesContractuales[index].Actividad = this.actividadesContractualesIniciales[index].Actividad ? this.actividadesContractualesIniciales[index].Actividad : null;
					this.actividadesContractuales[index].FechaInicial = this.actividadesContractualesIniciales[index].FechaInicial ? new Date(this.actividadesContractualesIniciales[index].FechaInicial) : null;
					this.actividadesContractuales[index].FechaFinal = this.actividadesContractualesIniciales[index].FechaFinal ? new Date(this.actividadesContractualesIniciales[index].FechaFinal) : null;
					// } else {
					// 	this.actividadesContractuales = [];
					// }
				}
				$timeout(function () {
					utilidades.mensajeSuccess("", false, () => { console.log('Restablecer actividades precontractuales'); }, null, "Se ha cancelado la edición.")

				}, 600);

			}, null, "Aceptar", "Cancelar", "Los datos que posiblemente haya diligenciado se perderán.");
		}

		this.infoModalidades = () => utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Modalidades, </span><br /> <span class='tituhori' >Los tipos de modalidad de contratacion.</span>");
		this.infoPlantilla = () => utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Descargar plantilla, </span><br /> <span class='tituhori' >Plantilla para cargar actividades.</span>");

		this.parametros = {};

		function ObtenerModalidadContratacion() {

			return cronogramavigfuturaServicio.ObtenerModalidadContratacion(0)
				.then((respuesta) => {
					if (respuesta.data != null && respuesta.data != "") {
						console.log(this);
						this.modalidadesContratacion = respuesta.data;
					}
					if (this.confirmarCancelarPrecontractual) {
						setTimeout(utilidades.mensajeSuccess("", false, () => { console.log('Continuar') }, () => { console.log('Cancelar') }, "Los datos han sido cambiados a modalidad"), 0);
					}
					this.obtenerModalidades(0);
				});
		}

		this.init = () => {
			this.ObtenerModalidadContratacion();

			this.notificacionvalidacion({ handler: this.notificacionValidacionPadre, nombreComponente: this.nombreComponente, esValido: true });
		};

		this.seleccionarModalidad = (id) => {
			var modalidadSeleccionada = this.modalidadesContratacion.find(c => c.Id === id)
			document.getElementById('Modalidad').innerHTML = modalidadSeleccionada.Nombre;
			console.log('nueva modalidad', id);
			this.modalidadContratacionSeleccionada = id;
		}


		function cambiarModalidad(modalidadContratacionId) {

			if (this.cambioModalidad) {
				utilidades.mensajeWarning("¿Está seguro de continuar?", () => {
					console.log('continuar');
					this.eliminarActividades = true;
					this.obtenerModalidades(modalidadContratacionId);
					this.modalidadContratacionIdAnterior = this.modalidadContratacionId;
					setTimeout(() => utilidades.mensajeSuccess("", false, () => { console.log('Restablecer actividades precontractuales'); }, null, `los datos han sido cambiados a modalidad ${this.modalidadesContratacion.filter(m => m.Id == modalidadContratacionId)[0].Nombre}`), 300);
				}, () => {
					this.seleccionarModalidad(this.modalidadContratacionId);

					console.log('cancelar');

				}, "Aceptar", "Cancelar", "Los datos que posiblemente haya diligenciado y guardado en el capítulo 'Cronograma' se perderán.");

			} else {
				this.obtenerModalidades(modalidadContratacionId);
			}
		}

		this.validarFechas = (sufijo, datos) => {
			const fechaActual = moment();
			if (!datos) { return false };
			var errorValidacion = false;
			var mensajesError = new Set();
			for (let index = 0; index < datos.length; index++) {
				const actividad = datos[index];

				const inputFechaInicial = document.getElementById(`fechaInicial${sufijo}${index}`);
				const fechaInicial = moment(actividad.FechaInicial);
				const fechaFinal = moment(actividad.FechaFinal);

				const inputFechaFinal = document.getElementById(`fechaFinal${sufijo}${index}`);
				const inputActividad = document.getElementById(`actividad${sufijo}${index}`);
				const filaError = document.getElementById(`errorFila${sufijo}${index}`);


				inputFechaInicial.className = 'form-control editInputDNP';
				inputFechaFinal.className = 'form-control editInputDNP';
				inputActividad.className = 'form-control editInputDNP';
				filaError.style.display = 'none';

				if (!fechaInicial.isValid() || actividad.FechaInicial == undefined) {
					inputFechaInicial.className = 'form-control editInputErrorDNP';
					filaError.style.display = 'inline';
					errorValidacion = true;
					mensajesError.add("La fecha estimada de inicio es inválida");
				}
				if (!fechaFinal.isValid() || actividad.FechaFinal == undefined) {
					inputFechaFinal.className = 'form-control editInputErrorDNP';
					filaError.style.display = 'inline';
					errorValidacion = true;
					mensajesError.add("la fecha estimada fin es inválida");
				}
				if (fechaInicial.year() < fechaActual.year()) {
					inputFechaInicial.className = 'form-control editInputErrorDNP';
					filaError.style.display = 'inline';
					errorValidacion = true;
					mensajesError.add('La fecha estimada de inicio debe ser mayor o igual al año actual');
				}
				if (fechaFinal.year() > +this.FechaHorizonte) {
					inputFechaFinal.className = 'form-control editInputErrorDNP';
					filaError.style.display = 'inline';
					errorValidacion = true;
					mensajesError.add(`La fecha estimada de finalización no puede superar el horizonte del proyecto (${this.FechaHorizonte})`);
				}
				if (fechaFinal.isBefore(fechaInicial)) {
					inputFechaFinal.className = 'form-control editInputErrorDNP';
					filaError.style.display = 'inline';
					errorValidacion = true;
					mensajesError.add(`La fecha estimada de finalización no puede ser menor que la fecha estimada de inicio`);
				}
				if (!actividad.Actividad) {
					inputActividad.className = 'form-control editInputErrorDNP';
					filaError.style.display = 'inline';
					errorValidacion = true;
					mensajesError.add(`La descripción de la actividad es obligatoria`);

				}
			}

			return { errorValidacion, mensajesError: Array.from(mensajesError) };
		}

		this.guardarPrecontractual = () => {
			var actualizarActividaes = {
				proyectoId: actividadesIniciales.ProyectoId,
				tramiteId: this.TramiteId,
				actividadesPreContractuales: null,
				actividadesContractuales: null
			}
			this.mensajesErrorPre = [];
			const validarPrecontractuales = this.validarFechas('Pre', this.actividadesPrecontractuales);
			if (validarPrecontractuales.errorValidacion) {
				utilidades.mensajeError('revise los campos señalados y valide nuevamente.', () => { }, 'Hay datos que presentan inconsistencias');
				this.mensajesErrorPre = validarPrecontractuales.mensajesError;
				return;
			};
			actualizarActividaes.actividadesPreContractuales = this.actividadesPrecontractuales;

			cronogramavigfuturaServicio.ActualizarActividadesCronograma(actualizarActividaes).then((response) => {
				if (response.data && (response.statusText === "OK" || response.status === 200)) {
					new utilidades.mensajeSuccess("Los datos han sido guardados con exito", false, false, false);
					this.editarPrecontractual = true;
					this.obtenerModalidades(this.modalidadContratacionId);
				}
				else {
					new utilidades.mensajeError("Error al realizar la operación");
				}

			});

		}

		this.guardarContractual = () => {
			var actualizarActividaes = {
				proyectoId: actividadesIniciales.ProyectoId,
				tramiteId: this.TramiteId,
				actividadesPreContractuales: null,
				actividadesContractuales: null,
				eliminarActividadesContractuales: null
			}
			this.mensajesErrorCon = [];
			const validarContractuales = this.validarFechas('Con', this.actividadesContractuales);
			if (validarContractuales.errorValidacion) {
				utilidades.mensajeError('revise los campos señalados y valide nuevamente.', () => { }, 'Hay datos que presentan inconsistencias');
				this.mensajesErrorCon = validarContractuales.mensajesError;
				return;
			};
			actualizarActividaes.actividadesContractuales = this.actividadesContractuales;
			actualizarActividaes.eliminarActividadesContractuales = this.EliminarActividadesContractuales;
			cronogramavigfuturaServicio.ActualizarActividadesCronograma(actualizarActividaes).then((response) => {
				if (response.data && (response.statusText === "OK" || response.status === 200)) {
					new utilidades.mensajeSuccess("Los datos han sido guardados con exito", false, false, false);
					this.editarContractual = true;
					this.EliminarActividadesContractuales = [];
					this.obtenerModalidades(this.modalidadContratacionId);

					// this.actividadesContractualesIniciales = [...response.data.ActividadesContractuales];
					// this.actividadesContractuales = [...response.data.ActividadesContractuales];
				} else {
					new utilidades.mensajeError("Error al realizar la operación");
				}
			});
		}

		this.editar = function () {
			this.isEdicion = true;
			this.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
		}

		this.obtenerModalidades = (modalidadContratacionId) => {
			this.modalidadContratacionId = modalidadContratacionId;
			// Restablecer valores 
			this.actividadesPrecontractuales = [];
			this.actividadesPrecontractualesIniciales = [];
			this.actividadesContractuales = [];
			this.actividadesContractualesIniciales = [];
			this.cambioModalidad = true;
			cronogramavigfuturaServicio.apiBackBoneObtenerActividadesPrecontractualesProyectoTramite(modalidadContratacionId, this.idProyecto, this.TramiteId, this.eliminarActividades)
				.then((actividades) => {
					console.log(actividades);

					if (actividades) {
						actividadesIniciales = actividades;
						const hayActividadesPrecontractuales = actividades.ActividadesPreContractuales && actividades.ActividadesPreContractuales.length > 0 ? true : false;
						if (hayActividadesPrecontractuales) {
							for (const precontractual of actividades.ActividadesPreContractuales) {
								precontractual.FechaInicial = precontractual.FechaInicial ? new Date(precontractual.FechaInicial) : null;
								precontractual.FechaFinal = precontractual.FechaFinal ? new Date(precontractual.FechaFinal) : null;
							}
							this.actividadesPrecontractuales = actividades.ActividadesPreContractuales;
							this.actividadesPrecontractualesIniciales = JSON.parse(JSON.stringify(this.actividadesPrecontractuales));
						}
						const hayActividadesContractuales = actividades.ActividadesContractuales && actividades.ActividadesContractuales.length > 0 ? true : false;
						if (hayActividadesContractuales) {
							for (const contractual of actividades.ActividadesContractuales) {
								contractual.FechaInicial = contractual.FechaInicial ? new Date(contractual.FechaInicial) : null;
								contractual.FechaFinal = contractual.FechaFinal ? new Date(contractual.FechaFinal) : null;
								if (this.modalidadContratacionId === 0) {
									this.modalidadContratacionId = contractual.ModalidadContratacionId;
								}
							}
							this.actividadesContractuales = actividades.ActividadesContractuales;
							this.actividadesContractualesIniciales = JSON.parse(JSON.stringify(this.actividadesContractuales));

						}
						this.verActividades = hayActividadesPrecontractuales || hayActividadesContractuales;
						if (!this.verActividades) {
							cronogramavigfuturaServicio.ObtenerModalidadContratacionVigenciasFuturas(this.idProyecto, this.TramiteId)
								.then((modalidad) => {
									if (modalidad !== null && modalidad !== undefined) {
										this.modalidadContratacionId = modalidad.data;
										this.seleccionarModalidad(this.modalidadContratacionId);
										this.verActividades = true;
										//var vMovilidad = document.getElementById("vModalidad");
										//if (vMovilidad != undefined) {
										//	vMovilidad.classList.add('hidden');
										//}
									}

								})
						}
						else {
							if (this.modalidadContratacionId == 0) {
								if (hayActividadesPrecontractuales) {
									this.modalidadContratacionId = this.actividadesPrecontractuales[0].ModalidadContratacionId;
									this.seleccionarModalidad(this.modalidadContratacionId);

								}

							}


						}
						if (this.actividadesContractuales.length === 0) {
							this.eliminarCapitulosModificados();
						}
						else {
							this.guardarCapituloModificado();
						}

					}
				});
		}

		this.eliminarRegistro = (index) => {
			if (this.actividadesContractuales[index].CronogramaId !== null) {
				this.EliminarActividadesContractuales.push(this.actividadesContractuales[index]);
			}
			this.actividadesContractuales.splice(index, 1);
		}
		//#region Archivo

		this.descargarPlantilla = () => {
			if (!this.modalidadContratacionId) {
				utilidades.mensajeError("Se debe seleccionar una modalidad de contratación para descargar la plantilla");
				return;
			}
			//if (this.actividadesContractuales.length == 0) {
			//	utilidades.mensajeError("No hay actividades Contractuales disponibles no se puede descargar la plantilla");
			//	return;
			//}
			//var dataPrecontractual = this.actividadesContractuales.map(x => ({ id: null, cronogramaId: x.CronogramaId, Actividad: x.Actividad, FechaInicial: moment(x.FechaInicial).format('YYYY/MM/DD HH:mm:ss'), FechaFinal: moment(x.FechaFinal).format('YYYY/MM/DD') }));
			//console.log(dataPrecontractual);
			//var hojaPrecontractual = XLSX.utils.json_to_sheet(dataPrecontractual, { dateNF: 'YYYY/MM/DD HH:mm:ss', cellDates: true });
			//hojaPrecontractual['!cols'] = [];
			//hojaPrecontractual['!cols'][0] = { hidden: true };
			//hojaPrecontractual['!cols'][1] = { hidden: true };

			var dataContractual = [
				{ Actividad: "", FechaInicial: '', FechaFinal: '' }
			];
			var hojaContractual = XLSX.utils.json_to_sheet(dataContractual);

			var libro = XLSX.utils.book_new();
			//XLSX.utils.book_append_sheet(libro, hojaPrecontractual, "PreContractual");
			XLSX.utils.book_append_sheet(libro, hojaContractual, "Contractual");
			XLSX.writeFile(libro, "Actividades-Contractuales.xlsx");
		}
		this.importarArchivo = (event) => {
			console.log(event);
		}

		this.validarArchivo = () => {
			if (this.files.length) {
				this.parseArchivo(this.files[0]).then(datos => {
					this.datosImportacion = datos;
					this.esArchivoValido = true;
					if (!this.validarCampos(datos)) {
						utilidades.mensajeSuccess("Proceda a cargar el archivo para que quede registrado en el sistema", false, () => { console.log('Restablecer actividades precontractuales'); }, null, "Validación de carga exitosa");
					} else {
						utilidades.mensajeError("El archivo adjuntado debe ser exactamente la plantilla para carga masiva, contener las celdas que allí se diseñaron, y estar diligenciado.", () => { this.limpiarArchivo(); }, "La validación no cumple los parámetros");
					};
				}, error => utilidades.mensajeError("El archivo adjuntado debe ser exactamente la plantilla para carga masiva, contener las celdas que allí se diseñaron, y estar diligenciado.", () => { this.limpiarArchivo(); }, "La validación no cumple los parámetros")

				);
			}
		}

		this.cargarArchivo = () => {
			if (this.datosImportacion) {
				try {
					this.actualizarActividades(this.datosImportacion);
					this.limpiarArchivo();
					this.HabilitaEditarContractual(false);
					utilidades.mensajeSuccess("Usted puede visualizarlos en la parte inferior de la tabla de actividades contractuales.", false, () => { }, null, "Los datos fueron cargados con éxito");

				} catch (error) {
					utilidades.mensajeError("Error al cargar el archivo");
				}
			}
		}
		this.validarCampos = (datos) => {
			var esInvalido = false;
			// validar estructura
			if (datos.precontractuales.length) {
				let camposPrecontractual = JSON.stringify(Object.keys(datos.precontractuales[0]));
				if (JSON.stringify(['id', 'Actividad', 'FechaInicial', 'FechaFinal']) !== camposPrecontractual) {
					esInvalido = true;
					console.log('columnas del archivo no coinciden, hoja precontractual');
				}
			} else {
				esInvalido = true;
				console.log('No hay datos precontractuales');
			}
			if (datos.contractuales.length) {
				let camposContractual = JSON.stringify(Object.keys(datos.contractuales[0]));
				if (JSON.stringify(['Actividad', 'FechaInicial', 'FechaFinal']) !== camposContractual) {
					esInvalido = true;
					console.log('columnas del archivo no coinciden, hoja contractual');
				}
			}
			// for (let precontractual of datos.precontractuales) {
			// 	var fechaInicial = moment(precontractual.FechaInicial);
			// 	var fechaFinal = moment(precontractual.FechaFinal);
			// 	if (!fechaInicial.isValid() || !fechaFinal.isValid()) {
			// 		esInvalido = true;
			// 		break;
			// 	};
			// }
			// for (let precontractual of datos.contractuales) {
			// 	var fechaInicial = moment(precontractual.FechaInicial);
			// 	var fechaFinal = moment(precontractual.FechaFinal);
			// 	if (!fechaInicial.isValid() || !fechaFinal.isValid() || !precontractual.Actividad) {
			// 		esInvalido = true;
			// 		break;
			// 	};
			// }
			return esInvalido;
		}
		this.actualizarActividades = (datos) => {
			//for (let precontractual of datos.precontractuales) {
			//	const actividad = this.actividadesPrecontractuales.find(pre => pre.ActividadPreContractualId == precontractual.id && pre.CronogramaId == precontractual.cronogramaId);
			//	actividad.FechaInicial = precontractual.FechaInicial ? new Date(precontractual.FechaInicial) : null;
			//	actividad.FechaFinal = precontractual.FechaFinal ? new Date(precontractual.FechaFinal) : null;
			//}
			for (let contractual of datos.contractuales) {
				//TODO: preguntar si se actualizan las actividades
				// const actividad = this.actividadesContractuales.find(pre => pre.ActividadPreContractualId == precontractual.id && pre.CronogramaId == precontractual.cronogramaId);
				contractual.FechaInicial = contractual.FechaInicial ? new Date(contractual.FechaInicial) : null;
				contractual.FechaFinal = contractual.FechaFinal ? new Date(contractual.FechaFinal) : null;
				this.actividadesContractuales.push({
					CronogramaId: null,
					ModalidadContratacionId: this.modalidadContratacionId,
					ActividadPreContractualId: null,
					Actividad: contractual.Actividad,
					FechaInicial: contractual.FechaInicial,
					FechaFinal: contractual.FechaFinal
				});
			}
		};

		this.limpiarArchivo = () => {
			this.files = [];
			this.esArchivoValido = false;
			document.getElementById('subirArchivoRepro').value = '';
		}

		this.parseArchivo = (file) => {
			return new Promise((resolve, reject) => {
				try {
					var datosParseados = [];
					let arrayBuffer;
					if (file) {
						const fileReader = new FileReader();
						fileReader.readAsArrayBuffer(file);
						fileReader.onload = (e) => {
							arrayBuffer = fileReader.result;
							const data = new Uint8Array(arrayBuffer);
							const arr = new Array();
							for (let i = 0; i != data.length; ++i) {
								arr[i] = String.fromCharCode(data[i]);
							}
							const bstr = arr.join('');
							const workbook = XLSX.read(bstr, { type: 'binary', cellText: false, cellDates: true });
							var contractuales = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { raw: false, cellText: false, cellDates: true });
							//var contractuales = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[1]], { raw: false, cellText: false, cellDates: true });
							// se rrecorren las hojas
							// validacion estructura
							if (workbook.SheetNames[0] !== 'Contractual') {
								console.log('Error estructura: hojas no coinciden');
								reject('Error estructura');
							} else {
								const sheets = { contractuales };
								resolve(sheets);
							}
							// this.inputFile.nativeElement.value = '';
						};
					} else {
						var datosArchivo = null;
						resolve(null)
					}
				} catch (error) {
					reject(error);
				}
			});
		}
		//#endregion
		/****************** inicio Grabar luneas */
		this.ObtenerSeccionCapitulo = () => {
			const span = document.getElementById('id-capitulo-reprogramacionvfcronograma');
			this.seccionCapitulo = span.textContent;
		}

		this.guardarCapituloModificado = () => {
			this.ObtenerSeccionCapitulo();
			var data = {
				//ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
				Justificacion: "",
				SeccionCapituloId: this.seccionCapitulo,
				InstanciaId: $sessionStorage.idInstancia,
				Modificado: 1,
				cuenta: 1
			}
			justificacionCambiosServicio.guardarCambiosFirme(data)
				.then((response) => {
					if (response.data.Exito) {
						this.guardadoevent({ nombreComponenteHijo: this.nombreComponente });
					}
					else {
						utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
					}
				})
				.catch(error => console.log(error));
		}

		this.eliminarCapitulosModificados = () => {
			this.ObtenerSeccionCapitulo();
			var data = {
				//ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
				Justificacion: "",
				SeccionCapituloId: this.seccionCapitulo,
				InstanciaId: $sessionStorage.idInstancia,

			}
			justificacionCambiosServicio.eliminarCapitulosModificados(data)
				.then((response) => {
					if (response.data.Exito) {
						this.guardadoevent({ nombreComponenteHijo: this.nombreComponente });
					}
					else {
						utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
					}
				})
				.catch(error => console.log(error));
		}

		this.notificacionValidacionPadre = (errores) => {
			//console.log("Validación  - CD Pvigencias futuras");
			this.limpiarErrores(errores);
			var isValid = true;
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == this.nombreComponente);
				var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
				if (erroresJson != undefined) {
					isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson[this.nombreComponente].forEach(p => {

							if (this.errores[p.Error] != undefined) this.errores[p.Error](p.Descripcion);
						});
					}
				}
				this.notificacionestado({ nombreComponente: this.nombreComponente, esValido: isValid });
			}
		}

	}

	angular.module('backbone').component('cronogramaModificarFormulario', {
		templateUrl: "src/app/formulario/ventanas/comun/cronograma/cronogramaModificarFormulario.html",
		controller: cronogramaModificarFormularioController,

		bindings: {

			callback: '&',
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
		},
	});

	//angular.module('backbone').directive('inputModalidad', function () {
	//	return {
	//		restrict: 'E',
	//		scope: {
	//			ngModel: '=',
	//			ngChange: '&',
	//			type: '@'
	//		},
	//		link: function (scope, element, attrs) {
	//			if (scope.type != undefined && scope.type.toLowerCase() != 'file') {
	//				return;
	//			}
	//			element.bind('change', function () {
	//				let files = element[0].files;
	//				scope.ngModel = files;
	//				scope.$apply();
	//				scope.ngChange();
	//			});
	//		}
	//	}

	//})

	angular.module('backbone').component('json', {
		bindings: {
			valor: '<',
		},
		template: `
		<style>
			pre {
				text-align: left;
			}
			.contenedor {
				position: fixed; 
				top:10px;
				right:10px; 
				z-index:9000; 
				background-color: white;
				overflow: scroll;
				height:100vh;
			}
			.btn-emoji {
				cursor: pointer;
				background-color: #eee;
				padding:5px;
				border-radius: 5px; 
				text-align: center;   
				width:100%;   
				margin-top:5px;     
			}
	
		</style>
		<div class='contenedor'>
			<div class='btn-emoji' ng-click="$ctrl.toogle()">😎​</div>
			<div class='btn-emoji' ng-if='$ctrl.mostrarJson' ng-click="$ctrl.actualizar()">➡️​</div>
			<pre id="codigo" ng-if='$ctrl.mostrarJson' contenteditable="true">{{$ctrl.formatJson($ctrl.valor)}}</pre>
		</div>
		`,
		controller: function () {
			this.ejecutar = '';
			this.mostrarJson = false;
			this.toogle = () => this.mostrarJson = !this.mostrarJson;
			this.formatJson = (json) => JSON.stringify(json, undefined, 2);
			this.actualizar = () => {
				var codigo = document.getElementById('codigo');
				const objeto = JSON.parse(codigo.innerHTML)
				for (const key in objeto) {
					if (Object.hasOwnProperty.call(objeto, key)) {
						this.valor[key] = objeto[key];
					}
				}

			}
		}
	});
})();