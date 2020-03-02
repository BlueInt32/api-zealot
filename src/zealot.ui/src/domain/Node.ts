import { NodeType } from './NodeType';

export class Node {
  public id: string = '';
  public parentId: string | null = null;
  public isPristine: boolean = true;
  public children: Node[] = [];
  public type: NodeType = NodeType.Request;
  public attributes: any = {};
}

export class RequestNode extends Node {
  public httpMethod: string = '';
  public requestUrl: string = '';
}
