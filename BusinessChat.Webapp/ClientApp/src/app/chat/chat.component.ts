import { Component, OnInit } from "@angular/core";
import { ChatService } from "../services/chat.service";
import { Message } from "../models/Message";
import { AuthorizeService } from "../../api-authorization";
import { map } from "rxjs/operators";
@Component({
  selector: "app-chat",
  templateUrl: "./chat.component.html",
  styleUrls: ["./chat.component.css"],
})
export class ChatComponent implements OnInit {
  userName: string;
  msgDto: Message = new Message("", "");
  msgInboxArray: Message[] = [];
  constructor(
    private chatService: ChatService,
    private authorizeService: AuthorizeService
  ) {}

  ngOnInit(): void {
    this.chatService
      .retrieveMappedObject()
      .subscribe((receivedObj: Message) => {
        this.addToInbox(receivedObj);
      });
    this.chatService
      .getLastMessages(50)
      .then((messages) =>
        messages.forEach((m) =>
          this.addToInbox(new Message(m.username, m.message))
        )
      );
    this.authorizeService
      .getUser()
      .pipe(map((u) => u && u.name))
      .subscribe((name) => {
        this.userName = name as string;
        this.msgDto.Username = this.userName;
      });
  }

  send(): void {
    if (this.msgDto) {
      if (this.msgDto.Message.length === 0) {
        window.alert("You must add a message.");
        return;
      } else {
        let smessageDTO = {
          Username:  this.msgDto.Username,
          Message: this.msgDto.Message
        }
        this.chatService.broadcastMessage(smessageDTO);
        this.msgDto.Message = "";
      }
    }
  }

  addToInbox(obj: Message) {
    let newObj = new Message(obj.Username, obj.Message);
    if(this.msgInboxArray.length < 50) {
      this.msgInboxArray.push(newObj);
    } else {
      this.msgInboxArray.shift();
      this.msgInboxArray.push(newObj);
    }

  }
}
