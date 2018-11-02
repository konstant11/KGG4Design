    var AddSentMessage = function(msg)
    {
              var target = document.getElementById('chat-messages-panel');


        var temp = document.createElement('div');
      temp.className+="row msg_container base_sent"

              var sent =  "<div class=\"col-md-10 col-xs-10\">" +
                    "<div class=\"messages msg_sent\">" +
                        "<p>" +
                            msg.TheMessage.MessageContent +
                        "</p>" +
                        "<time datetime = \"" + msg.Timestamp +"\" > " + msg.ChatActor.ActorName + "</time>" +
                    "</div>" +
                "</div>" +
                "<div class=\"col-md-2 col-xs-2 avatar\">" +
                    "<img src = \"/images/chat/two_face.png\" class=\" img-responsive \" >" +
                "</div>";

            temp.innerHTML = sent;
            target.appendChild(temp);

    };

    var AddReceiveMessage = function(msg)
    {
        var target = document.getElementById('chat-messages-panel');
        var imgTarget="one_face.png";
        if(msg.ChatActor.ActorName === "Home Bot")
        {
            imgTarget = "bot_avatar.png";
        }

        var temp = document.createElement('div');
            temp.className+="row msg_container base_receive";
               var rcv =   "<div class=\"col-md-2 col-xs-2 avatar\">" +
                    "<img src = \"/images/chat/" + imgTarget +" \" class=\" img-responsive \">" +
                "</div>" +
                "<div class=\"col-md-10 col-xs-10\">" +
                   "<div class=\"messages msg_receive\">" +
                        "<p>" +
                            msg.TheMessage.MessageContent +
                        "</p>" +
                        "<time datetime = \"" + msg.Timestamp +"\" > " + msg.ChatActor.ActorName + "</time>" +
                    "</div>" +
                "</div>";// +
            temp.innerHTML = rcv;
            target.appendChild(temp);
    };
    var AddParticipants = function(msg)
    {   
       var target = document.getElementById('chat-room-area');
            target.innerHTML="";
       var room = JSON.parse(msg);
       room.forEach(function(item, index, array) {
           var temp = document.createElement('div'); 
           temp.className+="chat-room-area col-xs3, col-md-3";
            var  body = "<img src=\"/images/chat/two_face.png\" class=\" img-responsive \">"+
                           "<p>"+ item.ActorName + "</p>";

           if(item.ActorName === "Home Bot")
           {
                body = "<img src=\"/images/chat/bot_avatar.png\" class=\" img-responsive \">"+
                       "<p>"+ item.ActorName + "</p>";
           }
       temp.innerHTML = body;
       target.appendChild(temp);
        });
    };