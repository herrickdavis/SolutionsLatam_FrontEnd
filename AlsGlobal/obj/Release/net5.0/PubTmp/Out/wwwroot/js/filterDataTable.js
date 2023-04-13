class FilterDataTable {
    constructor(props) {
        props.element.addClass("table-filtered-jf");
        this._setDefaultValue(props);
        this._filter = [];
        this._inicialize();
        var generateGuid = function () {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
                .replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                    return v.toString(16);
                });
        }
        this.id = generateGuid();
        if (!this._anyMethodOnFilter())
            console.warn("Por favor asegurate que el metodo onFilter sea function");
    }
    _inicialize() {
        this._mouseInParent = false;
        this._addFilterHeader();
        if(this._props.reColorder){
            this._addReOrderHeader();
        }
        this._addEventBody();
    }
    _setDefaultValue(props) {
        if (props.element === undefined)
            throw Error("El elemento es obligatorio");
        this._props = props || {};
        this._props.language = (this._props.language || document.filterDataTableLanguage) || {};
        this._props.language.filterBy = this._props.language.filterBy || "Filtro: ";
        this._props.language.searchBy = this._props.language.searchBy || "Buscar: ";
        this._props.language.buttonFilter = this._props.language.buttonFilter || "Filtrar";
        this._props.language.cleanFilter = this._props.language.cleanFilter || "Limpiar Filtros";
        this._props.language.contains = this._props.language.contains || "Contiene:";
        this._props.language.notContains = this._props.language.notContains || "No contiene:";
        this._props.language.equalsTo = this._props.language.equalsTo || "Igual a:";
        this._props.language.notEqualsTo = this._props.language.notEqualsTo || "No igual a:";
        this._props.language.empty = this._props.language.empty || "Vacio";
        this._props.language.notEmpty = this._props.language.notEmpty || "No vacio";
        this._props.language.greaterThan = this._props.language.greaterThan || "Mayor que:";
        this._props.language.smallerThan = this._props.language.smallerThan || "Menor que:";
   
        this._props.headerFilter = this._props.headerFilter || [];
        this._props.refreshRemoveFilter = this._props.refreshRemoveFilter === undefined ? true : this._props.refreshRemoveFilter;
        this._props.setFilterAll = this._props.setFilterAll === undefined ? false : this._props.setFilterAll;

        this._props.reColorder =  this._props.reColorder || false;
    }
    _addEventBody() {
        var instance = this;
        $("body").on("click", function () {
            if (!instance.mouseInParent)
                instance._onDestroy();
        });
    }
    _getHeader() {
        var header = $(this._props.element).find("thead th");
        if (header === undefined)
            header = $(this._props.element).find("tr:first td");
        
        return header;
    }
    _containsInput(element) {
        return $(element).find("input").length <= 0;
    }
    _createTemplateHeader(header, property, currentFilter) {
        var div = document.createElement("DIV");
        div.className = "row";

        var divCabecera = document.createElement("DIV");
        divCabecera.className = "col-md-9 col-9";
        divCabecera.innerHTML = header.innerHTML;
        $(div).append(divCabecera);

        var divSpan = document.createElement("DIV");
        divSpan.className = "col-md-1 col-1";
        var instance = this;

        var span = document.createElement("span");
        span.className = "fas fa-filter";
        span.style.cursor = "pointer";
        span.style.float = "right";
        span.style.top = "3px";

        var filter = instance._filter.firstOrDefault(x => x.id === property);
        if (filter !== null) {
            span.classList.add("text-info");
            instance._filter.firstOrDefault(x => x.id === property).span = span;
        }
        var type = currentFilter?.type || "text";
        span.setAttribute("data-type", instance._getTypeFilter(type));
        
        span.onmouseover = function () { instance._onMouseOverDiv(); };
        span.onmouseout = function () { instance._onMouseOutDiv(); };
        span.onclick = function () { instance._addToolTip(span, property); };

        $(divSpan).append(span);
        $(div).append(divSpan);

        $(header).empty();
        $(header).append(div);
    }
    _addFilterHeader() {
        var heads = this._getHeader();
        var instance = this;

        $.each(heads, function (index, item) {
            if (instance._containsInput(item)) {
                var property = item.getAttribute("data-header") || index.toString();
                var currentFilter = instance._props.headerFilter.firstOrDefault(x => x.headerIndex == property);
                if (instance._props.setFilterAll) var addFilter = currentFilter?.filter === undefined ? true : currentFilter.filter;
                else var addFilter = currentFilter.filter === undefined ? false : currentFilter.filter;
                
                if (addFilter)
                    instance._createTemplateHeader(item, property, currentFilter);
            }
        });
        return this._getHeader();
    };
    _addReOrderHeader() {
        var heads = this._getHeader();
        var instance = this;
        $.each(heads, (index, value) => {
            var orderable = $(value).attr("data-orderable") || false;
            if(orderable){
                $(value).addClass("noselect");
                $(value).prop("draggable", true);
                value.ondragstart = function(event){ instance._onDragStart(event, this) };
                value.ondrop = function(event){ instance._onDrop(event, this) };
                value.ondragover = function(event){ instance._onDragOver(event, this) };
                $(value).on("dragover", function(event) { instance._onDragOver(event, this)});
            }
        })

       
    };
    _addToolTip(elemento, property) {
        this._onDestroy();
        this._mouseInParent = true;
        var div = document.createElement("DIV");
        div.style.maxHeight = "300px";
        div.style.position = "absolute";
        div.style.display = "inline-block";
        div.style.maxWidth = "180px";
        div.style.zIndex = 12;
        div.className = "pt-4 pb-4";
        div.style.border = "1px solid #ccc;";
        div.style.backgroundColor = "#f5f5f5";
        div.style.boxShadow = "0 5px 10px rgba(0,0,0,.2)";
        div.setAttribute("data-header", property);
        div.id = this.id;
        var container = $("div[data-container=true]") || $("body");
        container.append(div);
        div.style.top = `${($(elemento).offset().top - $(div).offset().top) + elemento.offsetHeight}px`;
        div.style.left = `${$(elemento).offset().left - 90}px`;

        var type = elemento.getAttribute("data-type");
        $(div).append(this._createSelectWithOptions(type));
        $(div).append(this._createText(type, property));
        $(div).append(this._createButton(elemento));
        this._setValues(property);
    };
    _createSelectWithOptions(type) {
        var instance = this;
        var div = document.createElement("DIV");
        div.className = "col-md-12";
        div.onmouseover = function () { instance._onMouseOverDiv(); };
        div.onmouseout = function () { instance._onMouseOutDiv(); };
        //Creamos Label
        var label = document.createElement("LABEL");
        label.innerText = instance._props.language.filterBy;
        label.onmouseover = function () { instance._onMouseOverDiv(); };
        label.onmouseout = function () { instance._onMouseOutDiv(); };
        $(div).append(label);

        //Creamos select con opciones
        var div2 = document.createElement("DIV");
        div2.onmouseover = function () { instance._onMouseOverDiv(); };
        div2.onmouseout = function () { instance._onMouseOutDiv(); };

        var select = document.createElement("SELECT");
        select.className = "form-control";
        select.name = "condition";
        // select.onchange = function () { instance._onChangeSelect(this); };
        select.onmouseover = function () { instance._onMouseOverDiv(); };
        select.onmouseout = function () { instance._onMouseOutDiv(); };
        if (type === "text" || type === "select") {

            var option1 = document.createElement("OPTION");
            option1.value = "contiene";
            option1.innerText = this._props.language.contains;
            select.appendChild(option1);

            var option2 = document.createElement("OPTION");
            option2.value = "no contiene";
            option2.innerText = this._props.language.notContains;
            select.appendChild(option2);
            var option3 = document.createElement("OPTION");
            option3.value = "igual a";
            option3.innerText = this._props.language.equalsTo;
            select.appendChild(option3);

            var option4 = document.createElement("OPTION");
            option4.value = "no igual a";
            option4.innerText = this._props.language.notEqualsTo;
            select.appendChild(option4);
            var option5 = document.createElement("OPTION");
            option5.value = "vacio";
            option5.innerText = this._props.language.empty;
            select.appendChild(option5);

            var option6 = document.createElement("OPTION");
            option6.value = "no vacio";
            option6.innerText = this._props.language.notEmpty;
            select.appendChild(option6);
        }

        if (type === "datetime") {
            var option3 = document.createElement("OPTION");
            option3.value = "igual a";
            option3.innerText = this._props.language.equalsTo;
            select.appendChild(option3);

            var option4 = document.createElement("OPTION");
            option4.value = "no igual a";
            option4.innerText = this._props.language.notEqualsTo;
            select.appendChild(option4);

            var option7 = document.createElement("OPTION");
            option7.value = "mayor que";
            option7.innerText = this._props.language.greaterThan;
            select.appendChild(option7);

            var option8 = document.createElement("OPTION");
            option8.value = "menor que";
            option8.innerText = this._props.language.smallerThan;
            select.appendChild(option8);
        }

        $(div2).append(select);
        $(div).append(div2);

        return div;
    }
    _createText(type, property) {
        var instance = this;
        var div = document.createElement("DIV");
        div.className = "col-md-12";
        div.onmouseover = function () { instance._onMouseOverDiv(); };
        div.onmouseout = function () { instance._onMouseOutDiv(); };
        //Creamos input
        var div2 = document.createElement("DIV");
        div2.className = "mt-3";
        div2.onmouseover = function () { instance._onMouseOverDiv(); };
        div2.onmouseout = function () { instance._onMouseOutDiv(); };

        if (type === "select") {
            var input = document.createElement("SELECT");
            input.setAttribute("type", "select");
            input.className = "form-control";
            input.name = "value";

            var filterSettings = instance._props.headerFilter.firstOrDefault(x => x.headerIndex === property) || {};
            if (filterSettings.values === undefined)
                throw Error("Es necesario la propiedad Values en tipos Select");

            filterSettings.values.forEach(item => {
                var option = document.createElement("OPTION");
                option.value = item.value;
                option.innerText = item.text;
                $(input).append(option);
            });
            input.onchange = function () { instance._onKeyUpSelect(this); };
        } else {
            var input = document.createElement("INPUT");
            input.setAttribute("type", "text");
            input.className = "form-control";
            input.name = "value";
            if (type === "datetime") {
                input.setAttribute("data-toggle", "datetimepicker");
                // input.onchange = function () { instance._onKeyUpSelect(this); };
            } else {
                // input.onkeyup = function () { instance._onKeyUpSelect(this); };
            }
        }

        input.onmouseover = function () { instance._onMouseOverDiv(); };
        input.onmouseout = function () { instance._onMouseOutDiv(); };

        input.setAttribute("placeholder", instance._props.language.searchBy);

        $(div2).append(input);
        $(div).append(div2);
        if (type === "datetime") {
            $(input).datetimepicker({
                defaultDate: new Date(),
                format: 'YYYY-MM-DD'
            });
        }
        return div;
    }
    _createButton(span) {
        var instance = this;
        var div = document.createElement("DIV");
        div.className = "col-md-12 mt-3 btn-group btn-group-toggle";
        div.onmouseover = function () { instance._onMouseOverDiv(); };
        div.onmouseout = function () { instance._onMouseOutDiv(); };
        var buttonFiltrar = document.createElement("BUTTON");
        buttonFiltrar.setAttribute("type", "button");
        buttonFiltrar.className = "btn btn-sm  btn-primary";
        buttonFiltrar.innerHTML = instance._props.language.buttonFilter;
        buttonFiltrar.onclick = function () {
            instance._agregarValorEnFiltro(span);
            instance._filtrar();
        };
        buttonFiltrar.onmouseover = function () { instance._onMouseOverDiv(); };
        buttonFiltrar.onmouseout = function () { instance._onMouseOutDiv(); };
        $(div).append(buttonFiltrar);

        var buttonLimpiar = document.createElement("BUTTON");
        buttonLimpiar.setAttribute("type", "button");
        buttonLimpiar.className = "btn btn-sm  btn-outline-secondary";
        buttonLimpiar.innerHTML = instance._props.language.cleanFilter;
        buttonLimpiar.onclick = function () { instance._limpiarFiltro(this); };
        buttonLimpiar.onmouseover = function () { instance._onMouseOverDiv(); };
        buttonLimpiar.onmouseout = function () { instance._onMouseOutDiv(); };

        $(div).append(buttonLimpiar);
        return div;
    }
    _onChangeSelect(element) {
        var parentDiv = $(element).closest("div[data-header]");
        var property = parentDiv.attr("data-header");
        if (!this._filter.any(x => x.id === property)) {
            this._filter.push({ id: property, condition: element.value, value: "" });
        }
        else {
            this._filter.firstOrDefault(x => x.id === property).condition = element.value;
        }
    }
    _onKeyUpSelect(element) {
        var parentDiv = $(element).closest("div[data-header]");
        var property = parentDiv.attr("data-header");
        if (property === undefined) return;
        if (!this._filter.any(x => x.id === property)) {
            var condition = $(parentDiv).find("select[name=condition]").val();
            this._filter.push({ id: property, condition: condition, value: element.value });
        }
        else {
            this._filter.firstOrDefault(x => x.id === property).value = element.value;
        }
    }
    _agregarValorEnFiltro(span) {
        var parentDiv = $("#" + this.id);
        var property = parentDiv.attr("data-header");
        if (property === undefined) return;


        var condition = $(parentDiv).find("[name=condition]").val();
        var value = $(parentDiv).find("[name=value]").val();

        if ((condition !== "vacio" && condition !== "no vacio") && value.trim() === "") return;

        if (!this._filter.any(x => x.id === property)) {
            this._filter.push({ id: property, condition: condition, value: value, span: span });
        }
        else {
            this._filter.firstOrDefault(x => x.id === property).value = value;
        }
        $(span).addClass("text-info");
    }
    _filtrar() {
        if (this._anyMethodOnFilter()) {
            this._onDestroy();
            return this._props.onFilter(this.getFilter(), this);
        }
        else console.error("No existe metodo onFilter");
    }
    _limpiarFiltro(element) {
        var parentDiv = $(element).closest("div[data-header]");
        var property = parentDiv.attr("data-header");
        var filtro = this._filter.firstOrDefault(x => x.id === property);
        if (filtro !== null) {
            $(filtro.span).removeClass("text-info");
        }
        this._filter.splice(this._filter.indexOf(filtro), 1);
        if (this._props.refreshRemoveFilter)
            this._filtrar();
        else this._onDestroy();
    }
    _onDestroy() {
        $("#" + this.id).remove();
    }
    _onMouseOverDiv() {
        this.mouseInParent = true;
    };
    _onMouseOutDiv() {
        this.mouseInParent = false;
    };
    _onDragStart(event, element){
        event.dataTransfer.setData("index", $(element).index());
    }
    _onDragOver(event, element){
        event.preventDefault();
        $(".drag__over").removeClass("drag__over");
        var indexElement = $(element).index();
        $.each( this._props.element.find("tr"), (index, value) =>{
           $(value.children[indexElement]).addClass("drag__over");
        })
    }
    _onDrop(event, element){
        event.preventDefault();
        $(".drag__over").removeClass("drag__over");
        var indexDrag = parseInt(event.dataTransfer.getData("index"));
        var currentIndex = $(element).index();
        $.each($(this._props.element).find("tr"), (index, value)=>{
            $(value).children().eq(currentIndex).after($(value).children().eq(indexDrag));
        });
        
        this._onAfterSort();
    }
    _onAfterSort(){
        if(typeof this._props.onAfterSort === "function")
        {
            var headers = this._getHeader();
            this._props.onAfterSort(headers);
        }
    }
    _anyMethodOnFilter() {
        return typeof this._props.onFilter === "function";
    }
    _setValues(property) {
        var filter = this.getFilter().firstOrDefault(x => x.id === property);
        if (filter !== null) {
            $("#" + this.id).find("select[name=condition]").val(filter.condition);
            $("#" + this.id).find("input[name=value]").val(filter.value);
        }
    }
    getFilter() {
        return this._filter;
    };
    _getTypeFilter(type) {
        switch (type.toLowerCase()) {
            case 'datetime':
            case 'date': return 'datetime';
            case 'text': return 'text';
            case 'select': return 'select';
            default: throw Error("No existe el tipo " + type + "en los filtros");
        }
    }
    refresh(element) {
        if (element === undefined)
            throw Error("Es necesario el elemento");
        this._props.element = element;
        this._inicialize(this._props);
    }
};
