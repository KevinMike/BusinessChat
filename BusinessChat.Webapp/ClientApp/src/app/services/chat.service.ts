import { Injectable, OnInit } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Message } from "../models/Message";
import { Observable, Subject } from "rxjs";
import { MessageFromApi } from "../models/MessageFromApi";

@Injectable({
  providedIn: "root",
})
export class ChatService {
  private connection;
  private receivedMessageObject: Message = new Message();
  private sharedObj = new Subject<Message>();
  private readonly POST_URL = "https://localhost:5001/api/chat";
  private readonly GET_URL = "https://localhost:5001/api/chat";

  constructor(private http: HttpClient) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5001/chatsocket")
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.connection.onclose(async () => {
      await this.start();
    });
    this.connection.on("ReceiveOne", (user, message) => {
      this.mapReceivedMessage(user, message);
    });
    this.start();
  }

  public async start() {
    try {
      await this.connection.start();
    } catch (err) {
      setTimeout(() => this.start(), 5000);
    }
  }

  private mapReceivedMessage(user: string, message: string): void {
    this.receivedMessageObject.Username = user;
    this.receivedMessageObject.Message = message;
    this.sharedObj.next(this.receivedMessageObject);
  }

  public broadcastMessage(msgDto: any) {
    this.http
      .post(this.POST_URL, msgDto)
      .subscribe((data) => console.log(data));
  }

  public retrieveMappedObject(): Observable<Message> {
    return this.sharedObj.asObservable();
  }

  public getLastMessages(numberOfLastMessages: number): Promise<Array<MessageFromApi>> {
    const params = new HttpParams().set('numberOfLastMessages', numberOfLastMessages.toString());
    return this.http.get<Array<MessageFromApi>>(this.GET_URL, { params: params }).toPromise();
  }
}
