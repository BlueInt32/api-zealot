import { Node } from '@/domain/Node';

export default class UpdateProjectParams {
  public id: string = '';
  public name: string = '';
  public path: string = '';
  public tree: Node | null = null;
}
