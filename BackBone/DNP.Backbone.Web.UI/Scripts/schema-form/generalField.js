function DivField() {
    this.key;
    this.htmlClass;
    this.type = "section";
    this.cols;
    this.align;
    this.items;
}
DivField.prototype.constructor = DivField;

function AccordionField() {
    this.key;
    this.htmlClass;
    this.type = "accordion";
    this.titles;
    this.sections;
    this.items;
}
AccordionField.prototype.constructor = AccordionField;

function GridField() {
    this.key;
    this.htmlClass;
    this.type = "grid";
    this.urlGet;
    this.data;
}
GridField.prototype.constructor = GridField;

function generalField() {
    this.id; // Control id 
    this.key; // The dot notatin to the attribute on the model
    this.title; // Title of field, taken from schema if available
    this.placeholder; // control placeholder 
    this.tooltip; // Control Tooltip 
    this.type; // Type of control fieldset = a fieldset with legend, section = just a div, actions = horizontal button list, can only submit and buttons as items, text = input with type text, textarea = a textarea, number = input type number, password = input type password, checkbox= a checkbox, checkboxes=list of checkboxes, select = a select (single value), submit = a submit button, button = a button, radios = radio buttons, radios-inline = radio buttons in one line, radiobuttons = radio buttons with bootstrap buttons, help = insert arbitrary html
    this.require; // Set true or false to require o not
    this.validationText; //Control text validation
    this.format; //Control format
    this.htmlClass; // CSS Class(es) to be added to the container div
    this.readonly;// Same effect as readOnly in schema. Put on a fieldset or array and their items will inherit it.  
    this.tabOrder;
    this.defaulthValue;
    this.visible = false;
    this.scripts;
    this.draw;
}



function TextField() {    
        
}
TextField.prototype = new generalField();
TextField.prototype.constructor = TextField;
TextField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "text";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.maxLenght = "";
    this.minLenght = "";
};

function TextField(index) {

    this.init();
    this.id = index;
    this.id = index;
    this.key = index;
    this.title = index;
    this.placeholder = index;
    this.tooltip = index;


}

function NumberField() {

    
}
NumberField.prototype = new generalField();
NumberField.prototype.constructor = NumberField;
NumberField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "number";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.decimalNum = "";
    this.maxNumber = "";
    this.minNumber = "";
};

function TextAreaField() {
    
}
TextAreaField.prototype = new generalField();
TextAreaField.prototype.constructor = TextAreaField;
TextAreaField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "textarea";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.maxLenght = "";
};

function SimpleCheckBoxField() {    
       
}
CheckBoxField.prototype = new generalField();
CheckBoxField.prototype.constructor = CheckBoxField;
CheckBoxField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "checkbox";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.titleMap = "{1:'SI', 2:'NO'}";
};

function SelectField() {    
    
}
SelectField.prototype = new generalField();
SelectField.prototype.constructor = SelectField;
SelectField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "select";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.titleMap = "";
    this.urlGet = "";
    this.valueSelect = "";
    this.textSelect = "";
};

function RadioButtonField() {    
    
}
RadioButtonField.prototype = new generalField();
RadioButtonField.prototype.constructor = RadioButtonField;
RadioButtonField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "radiobutton";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.titleMap = "";
    this.urlGet = "";
    this.valueSelect = "";
    this.textSelect = "";
};

function CheckBoxField() {   
}
CheckBoxField.prototype = new generalField();
CheckBoxField.prototype.constructor = CheckBoxField;
CheckBoxField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "checkbox";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.titleMap = "";
    this.urlGet = "";
    this.valueSelect = "";
    this.textSelect = "";
};

function AutocompleteField() {    
   
}
AutocompleteField.prototype = new generalField();
AutocompleteField.prototype.constructor = AutocompleteField;
AutocompleteField.prototype.init = function () {
    this.id = "";
    this.key = "";
    this.title = "";
    this.placeholder = "";
    this.tooltip = "";
    this.type = "autocomplete";
    this.require = "";
    this.validationText = "";
    this.format = "";
    this.htmlClass = "";
    this.readonly = "";
    this.tabOrder = "";
    this.defaulthValue = "";
    this.visible = false;
    this.scripts = "";
    this.draw = "";
    this.titleMap = "";
    this.urlGet = "";
    this.valueSelect = "";
    this.textSelect = "";
};






