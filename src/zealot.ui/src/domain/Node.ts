import { NodeType } from './NodeType';
import { HttpMethodEnum } from './HttpMethodEnum';

export class Node {
  public id: string = '';
  public parentId: string | null = null;
  public isPristine: boolean = true;
  public children: Node[] = [];
  public type: NodeType = NodeType.Request;
}

export class RequestNode extends Node {
  public httpMethod: HttpMethodEnum = HttpMethodEnum.GET;
  public endpointUrl: string = '';
}
