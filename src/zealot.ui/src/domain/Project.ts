import { Node, RequestNode } from '@/domain/Node';

export default class Project {
  public id: string = '';
  public name: string = '';
  public tree: Node = new Node();
}
