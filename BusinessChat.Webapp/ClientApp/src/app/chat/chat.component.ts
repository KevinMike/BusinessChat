import { Component, OnInit } from "@angular/core";
import { ChatService } from "../services/chat.service";
import { Message } from "../models/Message";

@Component({
  selector: "app-chat",
  templateUrl: "./chat.component.html",
})
export class ChatComponent implements OnInit {
  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.chatService
      .retrieveMappedObject()
      .subscribe((receivedObj: Message) => {
        this.addToInbox(receivedObj);
      });
    this.chatService
      .getLastMessages(50)
      .then(messages => messages.forEach(m => this.addToInbox(m)));
  }

  msgDto: Message = new Message();
  msgInboxArray: Message[] = [];

  send(): void {
    if (this.msgDto) {
      if (this.msgDto.user.length == 0 || this.msgDto.user.length == 0) {
        window.alert("Both fields are required.");
        return;
      } else {
        this.chatService.broadcastMessage(this.msgDto);
      }
    }
  }

  addToInbox(obj: Message) {
    let newObj = new Message();
    newObj.user = obj.user;
    newObj.msgText = obj.msgText;
    this.msgInboxArray.push(newObj);
  }
}
