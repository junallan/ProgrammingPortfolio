

var todo = {
  saveType: { DELETEALL:1, DELETECOMPLETED:2, YESNOITEMMARK: 3},
  data: [], // holder for todo list array
  mongoStorage: [],
  load: function () {
  // todo.load() : attempt to load todo list from local storage

    // Parse JSON
    // [1] = Task
    // [2] = Status : 0 not done, 1 completed, 2 cancelled
//localStorage.list = '[["a",0],["b",1],["c",2],["d",0],["e",1],["f",2]]';

    $.ajax({
      url:"/Home/GetToDoItems",
      method: 'GET',
      success: function(data){   
        var todoListFormatted = data.map(d => '["' + d.id + '","' + d.workItemDescription + '",' + d.state + ']');
     
        mongoStorage = "[" + todoListFormatted + "]";       
        todo.data = JSON.parse(mongoStorage);
       
        todo.list();
      }
    });
  },

  save: function (saveType) {
  // todo.save() : save the current data to mongo storage

    let workItems = [];

    for(i in todo.data){
      workItems.push({ id: todo.data[i][0], workItemDescription: todo.data[i][1], state: todo.data[i][2] });
    }
    
    $.ajax({
      url:"/Home/SaveToDoItems",
      method: 'POST',
      data: { 'items': workItems, 'saveType': saveType},
      success: function(data){   
        //debugger;     
        todo.load();
      }
    });  
    mongoStorage = JSON.stringify(todo.data);
    todo.list();
  },

  list: function () {
  // todo.list() : update todo list HTML

    // Clear the old list
    var container = document.getElementById("todo-list");
    container.innerHTML = "";

    // Rebuild list
    if (todo.data.length > 0) {
      var row = "", el = "";
      for (var key in todo.data) {
        // Row container
        row = document.createElement("div");
        row.classList.add("clearfix");
        row.dataset.id = key;

        // Item text
        el = document.createElement("div");
        el.classList.add("item");
        if (todo.data[key][2] == 1) {
          el.classList.add("done");
        }
        if (todo.data[key][2] == 2) {
          el.classList.add("cx");
        }
        el.innerHTML = todo.data[key][1];
        row.appendChild(el);

        // Add cancel button
        el = document.createElement("input");
        el.setAttribute("type", "button");
        el.value = "\u2716";
        el.classList.add("bdel");
        el.addEventListener("click", function () {
          todo.status(this, 2);
        });
        row.appendChild(el);

        // Add done button
        el = document.createElement("input");
        el.setAttribute("type", "button");
        el.value = "\u2714";
        el.classList.add("bdone");
        el.addEventListener("click", function () {
          todo.status(this, 1);
        });
        row.appendChild(el);

        // Add row to list
        container.appendChild(row);
      }
    }
  },

  add: function () {
  // todo.add() : add a new item

    todo.data.push([
      "",document.getElementById("todo-add").value, 0
    ]);
    document.getElementById("todo-add").value = "";
    todo.save(todo.saveType.YESNOITEMMARK);
  },

  status: function (el, stat) {
  // todo.status() : update item status

    var parent = el.parentElement;
    todo.data[parent.dataset.id][2] = stat;
    todo.save(todo.saveType.YESNOITEMMARK);
  },

  del: function (type) {
  // todo.del() : delete items

  if (confirm("Delete tasks?")) {
      // Delete all
      if (type == 0) {
        todo.data = [];
        todo.save(todo.saveType.DELETEALL);
      }
      // Filter, keep only not completed
      else {
        todo.data = todo.data.filter(row => row[2]==0);
        todo.save(todo.saveType.DELETECOMPLETED);
      }
    }
  }
};

// Page init
window.addEventListener("load", function () {
  document.getElementById("todo-da").addEventListener("click", function () {
    todo.del(0);
  });
  document.getElementById("todo-dc").addEventListener("click", function () {
    todo.del(1);
  });
  document.getElementById("todo-form").addEventListener("submit", function (evt) {
    evt.preventDefault();
    todo.add();
  });
  todo.load();
});